using my_books1.Data.Models;
using my_books1.Data.ViewModels;

namespace my_books1.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;

        public BooksService(AppDbContext context)
        {
            _context = context;
        }

        public void AddBookWithAuthors(BookVM book)
        {

            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Book.Add(_book);
            _context.SaveChanges();


            foreach (var id in book.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id   
                };

                _context.Books_Authors.Add(_book_author);
                _context.SaveChanges();
            }


        }

        public List<Book> GetAllBooks() => _context.Book.ToList();

        public BookWithAuthorsVM GetBookById(int bookId)
        {
            var _bookWithAuthors = _context.Book.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();

            return _bookWithAuthors;
        }

        /*
        public List<Book> Test(int id)
        {
            return _context.Book.Where(n => n.Id == id).ToList().OrderBy(n => n.Id).ToList();
        }

        public List<int> Method(string description)
        {
            return _context.Book.Where(n => n.Description == description).ToList().Select(n => n.Id).ToList().OrderBy(n => n).ToList();
        }
        */

        public Book UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Book.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }

            return _book;
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Book.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _context.Book.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}
