using System.Configuration;

namespace Gamify.Sdk.Data.Configuration
{
    public class GameDataSection : ConfigurationSection, IGameDataSection
    {
        public static GameDataSection GetConfiguration()
        {
            return ConfigurationManager.GetSection("gameData") as GameDataSection;
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
