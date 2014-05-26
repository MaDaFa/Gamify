using System.Configuration;

namespace Gamify.Data.Configuration
{
    public class GameDataConfiguration : ConfigurationSection, IGameDataConfiguration
    {
        public static GameDataConfiguration GetConfiguration()
        {
            return ConfigurationManager.GetSection("gameData") as GameDataConfiguration;
        }

        [ConfigurationProperty("connectionString", IsRequired = true, DefaultValue = "")]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("databaseName", IsRequired = true, DefaultValue = "")]
        public string DatabaseName
        {
            get { return (string)this["databaseName"]; }
            set { this["databaseName"] = value; }
        }
    }
}
