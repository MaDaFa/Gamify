namespace Gamify.Server.Configuration
{
    public interface IGamifyConfiguration
    {
        string IpAddress { get; }

        int Port { get; }

        int TimeoutHours { get; }

        int TimeoutMinutes { get; }

        int TimeoutSeconds { get; }
    }
}
