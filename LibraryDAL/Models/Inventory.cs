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
    public class Inventory
    {
        [XmlArrayItem("Book", typeof(Book))]
        public List<Book> BookList { get; set; }
    }
}
