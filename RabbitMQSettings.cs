namespace RabbitMqGenericFramwork
{
    public class RabbitMQSettings
    {
        public bool Enable { get; set; }
        public string? ConnectionString { get; set; }
        public Dictionary<string, RabbitMQReceiver> Receivers { get; set; }
    }

    public class RabbitMQReceiver
    {
        public string? Exchange { get; set; }
        public string? Queue { get; set; }
        public string? RoutingKey { get; set; }
    }

}
