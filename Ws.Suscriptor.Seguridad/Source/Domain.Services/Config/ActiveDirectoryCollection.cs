using System.Configuration;

namespace Domain.Services.Config
{
    [ConfigurationCollection(typeof(ServiceElement),AddItemName = "Service")]
    public class ActiveDirectoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)(element)).Key;
        }
    }
}
