using System.Configuration;

namespace Domain.Services.Config
{
    public class ServiceElement :ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return ((string)(base["key"]));
            }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Path
        {
            get
            {
                return ((string)(base["path"]));
            }
        }

        [ConfigurationProperty("user", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string User
        {
            get
            {
                return ((string)(base["user"]));
            }
        }

        [ConfigurationProperty("pass", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Pass
        {
            get
            {
                return ((string)(base["pass"]));
            }
        }


    }
}
