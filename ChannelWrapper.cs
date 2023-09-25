using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqGenericFramwork;

public class ChannelWrapper : IChannelWrapper
{
    public IModel Channel { get; }


    public ChannelWrapper(IConnection connection)
    {
        Channel = connection.CreateModel();
    }

    public void DeclareQueue(string queueName)
    {
        Channel.QueueDeclare(queue: queueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void BindQueue(string queueName, string exchange, string routingKey)
    {
        Channel.QueueBind(queue: queueName,
                           exchange: exchange,
                           routingKey: routingKey);
    }

    public void BasicConsume(string queueName, EventingBasicConsumer consumer)
    {
        Channel.BasicConsume(queue: queueName,
                              autoAck: true,
                              consumer: consumer);
    }
}
