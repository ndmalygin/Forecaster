public interface ISettings
{
    string mongodb_connection { get; set; }
    string rabbitmq_connection { get; set; }
    string api_key { get; set; }
    string[] cities { get; set; }
    int interval { get; set; }
}