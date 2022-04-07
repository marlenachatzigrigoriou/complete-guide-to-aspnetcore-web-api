namespace my_books1.Data.Models
{
    public class Publisher

    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public List<Book> Books { get; set; }
    }
}
