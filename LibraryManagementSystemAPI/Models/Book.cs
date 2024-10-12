using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystemAPI.Models
{
    // Book Model - Represents the Book data that the API manages
    public class Book
    {
        [Key]                           // There was no requirement to use the "Key" annotation as the field name says Id and .NET can identify it as the primary key
        [Column("id")]
        [Display(Name = "Book Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }    // This will act as the PK in the SQLite Database

        [Required]
        [Column("title")]
        [Display(Name = "Book Title")]
        [StringLength(150, ErrorMessage = "{0} length cannot exceed {1}.")]
        public string? Title { get; set; }   // Required Field

        [Required]
        [Column("author")]
        [Display(Name = "Author of the Book")]
        [StringLength(100, ErrorMessage = "{0} length cannot exceed {1}.")]
        public string? Author { get; set; }  // Required Field

        [Required]
        [Column("description")]
        [Display(Name = "About the Book")]
        [StringLength(250, ErrorMessage = "{0} length cannot exceed {1}.")]
        public string? Description { get; set; } // Required Field

    }
}
