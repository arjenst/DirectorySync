using System.Configuration;

namespace DirectorySync.Configuration
{
    public class DirectorySyncInstanceElement : ConfigurationElement
    {
        // Create a property to store the name of the Sage CRM Instance
        // - The "name" is the name of the XML attribute for the property
        // - The IsKey setting specifies that this field is used to identify
        //   element uniquely
        // - The IsRequired setting specifies that a value is required
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                // Return the value of the 'name' attribute as a string
                return (string)base["name"];
            }
            set
            {
                // Set the value of the 'name' attribute
                base["name"] = value;
            }
        }

        // Create a property to store the server of the Sage CRM Instance
        // - The "source" is the name of the XML attribute for the property
        // - The IsRequired setting specifies that a value is required
        [ConfigurationProperty("source", IsRequired = true)]
        public string Source
        {
            get
            {
                // Return the value of the 'source' attribute as a string
                return (string)base["source"];
            }
            set
            {
                // Set the value of the 'source' attribute
                base["source"] = value;
            }
        }

        // Create a property to store the installation name of the Sage CRM Instance
        // - The "destination" is the name of the XML attribute for the property
        // - The IsRequired setting specifies that a value isn't required
        [ConfigurationProperty("destination", IsRequired = true)]
        public string Destination
        {
            get
            {
                return (string)base["destination"];
            }
            set
            {
                base["destination"] = value;
            }
        }
    }
}
