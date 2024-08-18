using Application.Services.BackgroundServices.Helpers;
using Core.Mailing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.BackgroundServices;
public class EmailSenderBackgroundService : BackgroundService
{
    private readonly ILogger<EmailSenderBackgroundService> _logger;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;
    private readonly IMailService _mailService;
    private const string _exchangeName = "emailExchange";
    private const string _queueName = "emailQueue";

    public EmailSenderBackgroundService(IConfiguration configuration, ILogger<EmailSenderBackgroundService> logger, IMailService mailService)
    {
        _configuration = configuration;
        _mailService = mailService;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout, true, false);
        _channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        _channel.QueueBind(queue: _queueName,
                          exchange: _exchangeName,
                          routingKey: "");
        _channel.BasicQos(0, 1, false);

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        _channel.BasicConsume(queue: _queueName,
                          autoAck: false,
                          consumer: consumer);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new MailboxAddressConverter());

                var mail = JsonConvert.DeserializeObject<Mail>(message, settings);

                await _mailService.SendEmailAsync(mail!);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error processing message from queue.");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        _channel?.Close();
        _connection?.Close();
    }

    private IConnection CreateConnection()
    {
        var uri = _configuration.GetValue<string>("RabbitMQConfiguration:Uri");

        var factory = new ConnectionFactory
        {
            Uri = new Uri(uri!),
            UserName = _configuration.GetValue<string>("RabbitMQConfiguration:Username"),
            Password = _configuration.GetValue<string>("RabbitMQConfiguration:Password"),
            DispatchConsumersAsync = true,
        };

  
        return factory.CreateConnection();
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}

