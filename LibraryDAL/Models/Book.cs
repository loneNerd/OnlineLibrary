using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryDAL.Models
{
    [XmlRoot]
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Author { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public string PublicationDate { get; set; }

        [XmlElement]
        public string Publisher { get; set; }

        [XmlElement]
        public int Pages { get; set; }

        [XmlElement]
        public string ISBN10 { get; set; }

        [XmlElement]
        public int InStock { get; set; }
    }
}
