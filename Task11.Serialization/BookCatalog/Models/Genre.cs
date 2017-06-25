using System.Xml.Serialization;

namespace BookCatalog.Models
{
    public enum Genre
    {
        [XmlEnum(Name = "Computer")]
        Computer,

        [XmlEnum(Name = "Fantasy")]
        Fantasy,

        [XmlEnum(Name = "Romance")]
        Romance,

        [XmlEnum(Name = "Horror")]
        Horror,

        [XmlEnum(Name = "Science Fiction")]
        ScienceFiction
    }
}
