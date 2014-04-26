using System.Configuration;

namespace Gamify.Server.Configuration
{
    public class GamifyConfiguration : ConfigurationSection, IGamifyConfiguration
    {
        public static GamifyConfiguration GetConfiguration()
        {
            return ConfigurationManager.GetSection("gameConfiguration") as GamifyConfiguration;
        }

        [ConfigurationProperty("ipAddress", IsRequired = true, DefaultValue = "")]
        public string IpAddress
        {
            get { return (string)this["ipAddress"]; }
            set { this["ipAddress"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true, DefaultValue = 0)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("timeoutHours", IsRequired = true, DefaultValue = 0)]
        public int TimeoutHours
        {
            get { return (int)this["timeoutHours"]; }
            set { this["timeoutHours"] = value; }
        }

        [ConfigurationProperty("timeoutMinutes", IsRequired = true, DefaultValue = 0)]
        public int TimeoutMinutes
        {
            get { return (int)this["timeoutMinutes"]; }
            set { this["timeoutMinutes"] = value; }
        }

        [ConfigurationProperty("timeoutSeconds", IsRequired = true, DefaultValue = 0)]
        public int TimeoutSeconds
        {
            get { return (int)this["timeoutSeconds"]; }
            set { this["timeoutSeconds"] = value; }
        }

        string IGamifyConfiguration.IpAddress
        {
            get { return this.IpAddress; }
        }

        int IGamifyConfiguration.Port
        {
            get { return this.Port; }
        }

        int IGamifyConfiguration.TimeoutHours
        {
            get { return this.TimeoutHours; }
        }

        int IGamifyConfiguration.TimeoutMinutes
        {
            get { return this.TimeoutMinutes; }
        }

        int IGamifyConfiguration.TimeoutSeconds
        {
            get { return this.TimeoutSeconds; }
        }
    }
}
