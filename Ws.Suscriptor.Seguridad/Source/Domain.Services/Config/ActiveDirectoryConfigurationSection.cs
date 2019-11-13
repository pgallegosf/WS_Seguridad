using System.Configuration;

namespace Domain.Services.Config
{
    public class ActiveDirectoryConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("ActiveDirectoryServices")]
        public ActiveDirectoryCollection ServicesItems
        {
            get { return ((ActiveDirectoryCollection)(base["ActiveDirectoryServices"])); }
        }
    }
}
