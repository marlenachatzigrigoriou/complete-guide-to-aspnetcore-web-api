using Microsoft.EntityFrameworkCore;
using my_books1.Data;
using my_books1.Data.Models;
using my_books1.Data.Services;
using my_books1.Data.ViewModels;
using my_books1.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_books_tests
{
    public class PublishersServiceTests
    {

        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDBControllerTest")
            .Options;   
            
        AppDbContext context;
        PublishersService publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            publishersService = new PublishersService(context);

            SeedDataBase();
        }

        [Test]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5)); //it's 5 (not 6) -> the page size is by default 5
        }

        [Test]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "1", null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 1"));
        }

        [Test]
        public void GetAllPublishers_WithSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("name_desc", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 6"));
        }

        [Test]
        public void GetPublisherById_WithValidId_Test()
        {
            var result = publishersService.GetPublisherById(1);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
        }

        [Test]
        public void GetPublisherById_WithNullId_Test()
        {
            var result = publishersService.GetPublisherById(88);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddPublisher_WithException_Test()
        {
            var newPublisher = new PublisherVM()
            {
                Name = "123 oups"
            };

            Assert.That(() => publishersService.AddPublisher(newPublisher),
                Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Name starts with number"));
        }

        [Test]
        public void AddPublisher_WithoutException_Test()
        {
            var newPublisher = new PublisherVM()
            {
                Name = "valid name"
            };

            var result = publishersService.AddPublisher(newPublisher);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("valid"));
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public void GetPublisherData_Test()
        {
            var result = publishersService.GetPublisherData(1);

            Assert.That(result.BookAuthors, Is.Not.Empty);
            Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));

            var bookname = result.BookAuthors.OrderBy(n => n.BookName).FirstOrDefault().BookName;
            Assert.That(bookname, Is.EqualTo("Book 1 Title"));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDataBase()
        {
            var publishers = new List<Publisher>
            {
                    new Publisher() {
                        Id = 1,
                        Name = "Publisher 1"
                    },
                    new Publisher() {
                        Id = 2,
                        Name = "Publisher 2"
                    },
                    new Publisher() {
                        Id = 3,
                        Name = "Publisher 3"
                    },
                    new Publisher() {
                        Id = 4,
                        Name = "Publisher 4"
                    },
                    new Publisher() {
                        Id = 5,
                        Name = "Publisher 5"
                    },
                    new Publisher() {
                        Id = 6,
                        Name = "Publisher 6"
                    },
            };
            context.Publishers.AddRange(publishers);

            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    FullName = "Author 1"
                },
                new Author()
                {
                    Id = 2,
                    FullName = "Author 2"
                }
            };
            context.Authors.AddRange(authors);


            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                }
            };
            context.Book.AddRange(books);

            var books_authors = new List<Book_Author>()
            {
                new Book_Author()
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                },
                new Book_Author()
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                },
                new Book_Author()
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2
                },
            };
            context.Books_Authors.AddRange(books_authors);


            context.SaveChanges();
        }
    }


}