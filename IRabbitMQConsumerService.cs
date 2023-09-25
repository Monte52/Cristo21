namespace RabbitMqGenericFramwork
{
    public interface IRabbitMQConsumerService
    {
        void Consume(string exchange, string queue, string routingKey);
        void StartConsumers();
        void StartNewConsumersBasedOnNewSettings(RabbitMQSettings rabbitMQSettings);
    }
}
