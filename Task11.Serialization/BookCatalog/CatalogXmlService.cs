using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using BookCatalog.Interfaces;
using BookCatalog.Models;

namespace BookCatalog
{
    public class CatalogXmlService : ISerializationService
    {
        private const string FilePathKey = "booksFilePath";
        private readonly ISettingsProvider _settingsProvider;

        public CatalogXmlService(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public void Serialize(Catalog catalog)
        {
            string filePath = _settingsProvider.GetSetting(FilePathKey);

            var xml = new XmlSerializer(typeof(Catalog));
            
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                xml.Serialize(writer, catalog, GetCatalogNamespace());
            }

        }

        public Catalog Deserialize()
        {
            string filePath = _settingsProvider.GetSetting(FilePathKey);

            var xml = new XmlSerializer(typeof(Catalog));

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                return (Catalog) xml.Deserialize(reader);
            }
        }

        private XmlSerializerNamespaces GetCatalogNamespace()
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            string ns = string.Empty;

            var xmlRootAttribute = typeof(Catalog).GetCustomAttribute(typeof(XmlRootAttribute)) as XmlRootAttribute;

            if (xmlRootAttribute?.Namespace != null)
            {
                ns = xmlRootAttribute.Namespace;
            }

            namespaces.Add(string.Empty, ns);

            return namespaces;
        }
    }
}
