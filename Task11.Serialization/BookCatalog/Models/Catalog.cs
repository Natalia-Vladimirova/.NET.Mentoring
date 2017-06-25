using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookCatalog.Models
{
    [XmlRoot("catalog", Namespace = "http://library.by/catalog", IsNullable = false)]
    public class Catalog
    {
        [XmlAttribute(AttributeName = "date", DataType = "date")]
        public DateTime Date { get; set; }

        [XmlElement("book")]
        public List<Book> Books { get; set; }
    }
}
