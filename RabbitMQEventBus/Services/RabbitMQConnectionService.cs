using RabbitMQ.Client;

namespace MessagingService;

public interface IRabbitMQConnectionService
{ 
    Task<IChannel> GetChannelAsync();
}

public class RabbitMQConnectionService: IRabbitMQConnectionService
{
    private readonly ConnectionFactory _factory = new() { HostName = "localhost" };
    private IConnection _connection;
    private IChannel _channel;

    private async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is { IsOpen: true }) return _connection;
        _connection = await _factory.CreateConnectionAsync();
        return _connection;
    }

    public async Task<IChannel> GetChannelAsync()
    {
        if (_channel is { IsOpen: true }) return _channel;
        var connection = await GetConnectionAsync();
        _channel = await connection.CreateChannelAsync();
        return _channel;
    }
}