using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public interface IChannelWrapper
{
    IModel Channel { get; }
    void DeclareQueue(string queueName);
    void BindQueue(string queueName, string exchange, string routingKey);
    void BasicConsume(string queueName, EventingBasicConsumer consumer);
}
