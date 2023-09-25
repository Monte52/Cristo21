using RabbitMQ.Client;

namespace RabbitMqGenericFramwork
{
    public interface IConnectionFactoryWrapper
    {
        IConnection GetConnection();
    }
}
