using System;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class contains book info and using in as view model for web site.
    /// </summary>
    [XmlRoot]
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter an author")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a publication date")]
        [Display(Name = "Publication Date")]
        [DataType(DataType.DateTime)]
        public string PublicationDate { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a publisher")]
        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a number of pages")]
        [Display(Name = "Pages")]
        [Range(0, int.MaxValue, ErrorMessage = "Please, enter a correct number of pages")]
        public int Pages { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter a ISBN10")]
        [Display(Name = "ISBN")]
        public string ISBN10 { get; set; }

        [XmlElement]
        [Required(ErrorMessage = "Please, enter an amount in stock")]
        [Display(Name = "In Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "Please, enter correct amount in stock")]
        public int InStock { get; set; }

        //Short name for book preview in view model
        public string TrimName() => Name.Length > 50 ? Name.Substring(0, 50) + "..." : Name;
        //Short description for book preview in view model
        public string TrimDescription() => Description.Length > 250 ? Description.Substring(0, 250) + "..." : Description;
    }
}
