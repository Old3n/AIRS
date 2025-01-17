using AIRS.SharedLibrary.Logs;
using AIRS.UserProfileApi.Application.Dtos;
using AIRS.UserProfileApi.Application.Interfaces;
using AIRS.UserProfileApi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Text.Json;

namespace AIRS.UserProfileApi.Infrastructure.Messaging
{
    public class TestResultSubscriber : BackgroundService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _queue;

        public TestResultSubscriber(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _queue = configuration["RabbitMQ:Queue"]!;
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: _queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _queue, exchange: "trigger", routingKey: "test_result");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartListening();
            await Task.CompletedTask;
        }

        private void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    var testResult = JsonSerializer.Deserialize<TestPublishedDto>(message);
                    using var scope = _serviceProvider.CreateScope();
                    var userInfoRepository = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
                    var userInfo = new UserInfo
                    {
                        UserId = testResult!.UserId,
                        MbtiType = testResult.Result,
                    };
                    await userInfoRepository.AddUser(userInfo);
                }
                catch (Exception ex)
                {
                    LogExceptions.LogException(ex);
                }
            };

            _channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            StopListening();
            return base.StopAsync(stoppingToken);
        }

        private void StopListening()
        {
            _channel.Close();
            _connection.Close();
        }

        public new void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
            }
            _connection.Close();
            _connection.Dispose();
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        ~TestResultSubscriber()
        {
            Dispose();
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Log.Information("RabbitMQ Connection Shutdown");
        }
    }
}
