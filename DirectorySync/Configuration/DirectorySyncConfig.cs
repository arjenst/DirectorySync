using System.Configuration;

namespace DirectorySync.Configuration
{
    public class DirectorySyncConfig : ConfigurationSection
    {
        // Create a property that lets us access the collection
        // of DirectorySyncInstanceElements

        // Specify the name of the element used for the property
        [ConfigurationProperty("directories")]
        // Specify the type of elements found in the collection
        [ConfigurationCollection(typeof(DirectorySyncInstanceCollection))]
        public DirectorySyncInstanceCollection DirectorySyncInstances
        {
            get
            {
                // Get the collection and parse it
                return (DirectorySyncInstanceCollection)this["directories"];
            }
        }
    }
}