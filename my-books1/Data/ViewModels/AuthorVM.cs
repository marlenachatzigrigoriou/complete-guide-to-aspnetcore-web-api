namespace my_books1.Data.ViewModels
{
    public class AuthorVM
    {
        public string FullName { get; set; }
    }

    public class AuthorWithBooksVm
    {
        public string FullName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}
