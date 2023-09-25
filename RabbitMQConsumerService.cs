using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqGenericFramwork;
using System;
using System.Text;

public class RabbitMQConsumerService : IRabbitMQConsumerService
{
    private readonly IChannelWrapper _channelWrapper;
    private readonly RabbitMQSettings _rabbitMQSettings;
    private readonly HashSet<string> _runningQueues = new HashSet<string>();

    public RabbitMQConsumerService(IChannelWrapper channelWrapper, RabbitMQSettings rabbitMQSettings)
    {
        _channelWrapper = channelWrapper;
        _rabbitMQSettings = rabbitMQSettings;
    }
    public void StartConsumers()
    {
        if (!_rabbitMQSettings.Enable)
        {
            return;
        }

        foreach (var receiver in _rabbitMQSettings.Receivers)
        {
            var key = receiver.Key;
            var settings = receiver.Value;

        
            if (!_runningQueues.Contains(key))
            {
         
                Consume(settings.Exchange, settings.Queue, settings.RoutingKey);

           
                _runningQueues.Add(key);
            }
        }
    }
    public void Consume(string exchange, string queue, string routingKey)
    {
        _channelWrapper.DeclareQueue(queue);
        _channelWrapper.BindQueue(queue, exchange, routingKey);
        var consumer = new EventingBasicConsumer(_channelWrapper.Channel);  // Use the exposed IModel object
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received from {queue}: {message}");
        };

        _channelWrapper.BasicConsume(queue, consumer);
    }
    public void StartNewConsumersBasedOnNewSettings(RabbitMQSettings newSettings)
    {
        if (!newSettings.Enable)
        {
            return;
        }

        foreach (var receiver in newSettings.Receivers)
        {
            var key = receiver.Key;
            var settings = receiver.Value;

            // Check if this consumer is already running
            if (!_runningQueues.Contains(key))
            {
                // Start the consumer
                Consume(settings.Exchange, settings.Queue, settings.RoutingKey);

                // Mark this consumer as running
                _runningQueues.Add(key);
            }
        }
    }

}


