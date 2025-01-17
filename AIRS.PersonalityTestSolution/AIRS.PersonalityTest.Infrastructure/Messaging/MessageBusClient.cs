using AIRS.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using System.Text.Json;
using System.Text;
using AIRS.PersonalityTest.Application.Interfaces;

public class MessageBusClient : IMessageBus, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"],
            Port = int.TryParse(_configuration["RabbitMQ:Port"], out var port) ? port : 5672,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Direct);
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            LogExceptions.LogException(ex);
            throw;
        }
    }

    public void Dispose()
    {
        Log.Information("Disposing MessageBusClient...");
        if (_channel?.IsOpen == true)
        {
            _channel.Close();
        }
        _connection.Close();
        _connection.Dispose();
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Log.Warning("RabbitMQ Connection Shutdown");
    }
}
