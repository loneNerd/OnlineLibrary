using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class represent model when reader take book don't return yet.
    /// </summary>
    public class Order
    {
        private const int _penaltyPerDay = 10;
        private const int _expiredTime = 14; //Time to return book

        [Key]
        public int OrderID { get; set; }
        public Book Book { get; set; }
        public DBUser Reader { get; set; }
        public DBUser Librarian { get; set; }
        public DateTime OrderDay { get; set; } //Day when reader take book from library
        public DateTime CloseDay { get; set; } //Day when reader return book to library and pay his penalty if he has one
        public string Status { get; set; } //Status of order. "Active" - when reader take book, "Close" - when reader return book

        [NotMapped]
        public DateTime ExpiredDay //Final day when reader shoud return book to library
        {
            get { return OrderDay.AddDays(_expiredTime); }
        }
        
        [NotMapped]
        public int Penalty
        {
            get { return (CloseDay - ExpiredDay).TotalDays > 0 ? (int)(CloseDay - ExpiredDay).TotalDays * _penaltyPerDay : 0; }
        }
    }
}
