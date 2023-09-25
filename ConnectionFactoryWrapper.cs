using RabbitMQ.Client;
using RabbitMqGenericFramwork;

public class ConnectionFactoryWrapper : IConnectionFactoryWrapper
{
    private IConnection _connection;
    private readonly string _hostName;

    public ConnectionFactoryWrapper(string hostName)
    {
        _hostName = hostName;
    }

    public IConnection GetConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            factory.Ssl.Version = System.Security.Authentication.SslProtocols.Tls12;
            _connection = factory.CreateConnection();
        }
        return _connection;
    }
}
