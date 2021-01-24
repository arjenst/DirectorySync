using System.Configuration;

namespace DirectorySync.Configuration
{
    public class DirectorySyncInstanceCollection : ConfigurationElementCollection
    {
        // Create a property that lets us access an element in the
        // collection with the int index of the element
        public DirectorySyncInstanceElement this[int index]
        {
            get
            {
                // Gets the DirectorySyncInstanceElement at the specified
                // index in the collection
                return (DirectorySyncInstanceElement)BaseGet(index);
            }
            set
            {
                // Check if a DirectorySyncInstanceElement exists at the
                // specified index and delete it if it does
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                // Add the new DirectorySyncInstanceElement at the specified
                // index
                BaseAdd(index, value);
            }
        }

        // Create a property that lets us access an element in the
        // collecton with the name of the element
        public new DirectorySyncInstanceElement this[string key]
        {
            get
            {
                // Gets the DirectorySyncInstanceElement where the name
                // matches the string key specified
                return (DirectorySyncInstanceElement)BaseGet(key);
            }
            set
            {
                // Checks if a DirectorySyncInstanceElement exists with
                // the specified name and deletes it if it does
                if (BaseGet(key) != null)
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));

                // Adds the new DirectorySyncInstanceElement
                BaseAdd(value);
            }
        }

        // Method that must be overriden to create a new element
        // that can be stored in the collection
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectorySyncInstanceElement();
        }

        // Method that must be overriden to get the key of a
        // specified element
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DirectorySyncInstanceElement)element).Name;
        }
    }
}