namespace my_books1.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        //Navigation Properties
        public List<Book_Author> Book_Authors { get; set; }

    }
}
