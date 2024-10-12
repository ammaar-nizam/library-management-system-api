namespace LibraryManagementSystemAPI.Models
{
    // Book Model - Represents the Book data that the API manages
    public class Book
    {
        public long Id { get; set; }    // This will act as the PK in the SQLite Database
        public string Title { get; set; }   // Required Field
        public string Author { get; set; }  // Required Field
        public string Description { get; set; } // Required Field

    }
}
