using System;
using System.Xml.Serialization;

namespace BookCatalog.Models
{
    public class Book
    {
        [XmlAttribute(AttributeName = "id")]
        public string BookId { get; set; }

        [XmlElement(ElementName = "isbn", IsNullable = false)]
        public string Isbn { get; set; }

        [XmlElement(ElementName = "author", IsNullable = false)]
        public string Author { get; set; }

        [XmlElement(ElementName = "title", IsNullable = false)]
        public string Title { get; set; }

        [XmlElement(ElementName = "genre", IsNullable = false)]
        public Genre Genre { get; set; }

        [XmlElement(ElementName = "publisher", IsNullable = false)]
        public string Publisher { get; set; }

        [XmlElement(ElementName = "publish_date", DataType = "date", IsNullable = false)]
        public DateTime PublishDate { get; set; }

        [XmlElement(ElementName = "description", IsNullable = false)]
        public string Description { get; set; }

        [XmlElement(ElementName = "registration_date", DataType = "date", IsNullable = false)]
        public DateTime RegistrationDate { get; set; }
    }
}
