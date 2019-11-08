using System.ComponentModel.DataAnnotations;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class represent model when reader order book not take it yet.
    /// </summary>
    public class PreOrder
    {
        [Key]
        public int PreOrderID { get; set; }
        public Book Book { get; set; }
        public DBUser Reader { get; set; }
        public string Status { get; set; } //"Active" - when book been ordered, "Disable" - when reader take book or librarian refuse order
    }
}
