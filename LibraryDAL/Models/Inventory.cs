using System.Collections.Generic;
using System.Xml.Serialization;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class need to add books from xml file.
    /// </summary>
    [XmlRoot]
    public class Inventory
    {
        [XmlArrayItem("Book", typeof(Book))]
        public List<Book> BookList { get; set; }
    }
}
