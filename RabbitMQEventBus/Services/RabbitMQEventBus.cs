using System.Text;
using Domain.Core.EventBus;
using Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagingService;

public sealed class RabbitMQEventBus : IEventBus
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRabbitMQConnectionService _connectionService;
        private readonly AsyncRetryPolicy _retryPolicy;
        
        private readonly Dictionary<string, List<Type>> _handlers = new();
        private readonly List<Type> _eventTypes = new();

        public RabbitMQEventBus(IServiceScopeFactory serviceScopeFactory, IRabbitMQConnectionService connectionService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _connectionService = connectionService;
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine(
                            $"Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: {exception.Message}");
                    });
        }
        
        public async Task Publish<T>(T @event) where T : Event
        {
            var channel = await _connectionService.GetChannelAsync();

            var eventName = @event.GetType().Name;
            await channel.QueueDeclareAsync(eventName, durable: false, exclusive: false, autoDelete: false);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            await _retryPolicy.ExecuteAsync(async () =>
            {
                await channel.QueueDeclareAsync(eventName, durable: false, exclusive: false, autoDelete: false);
                await channel.BasicPublishAsync("", eventName, false, body);
            });
        }

        public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} is already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            await StartBasicConsume(eventName);
        }

        private async Task StartBasicConsume(string eventName)
        {
            var channel = await _connectionService.GetChannelAsync();
            
            await channel.QueueDeclareAsync(eventName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        await ProcessEvent(eventName, message);
                        await channel.BasicAckAsync(e.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing message: " + ex.Message);
                        throw;
                    }
                });
            };

            await channel.BasicConsumeAsync(eventName, autoAck: false, consumer);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.TryGetValue(eventName, out var subscriptions))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                foreach (var handlerType in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);
                    if (handler == null) continue;

                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    if (eventType == null) continue;

                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, [@event])!;
                }
            }
        }
    }