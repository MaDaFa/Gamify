using System;
using System.Configuration;

namespace Gamify.Sdk.Data.Configuration
{
    public class GameDataSection : ConfigurationSection, IGameDataSection
    {
        private static readonly Lazy<IGameDataSection> instance;

        static GameDataSection()
        {
            instance = new Lazy<IGameDataSection>(() =>
            {
                return ConfigurationManager.GetSection("gameData") as IGameDataSection;
            });
        }

        public static IGameDataSection Instance()
        {
            return instance.Value;
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
