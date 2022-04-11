using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using my_books1.Controllers;
using my_books1.Data;
using my_books1.Data.Models;
using my_books1.Data.Services;
using my_books1.Data.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    internal class PublishersControllerTests
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDBTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;
        PublishersController publishersController;


        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            publishersService = new PublishersService(context);
            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());

            SeedDataBase();
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortBySearchPageNr_ReturnOK_Test()
        {
            //1st page
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "Pub", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData.Count, Is.EqualTo(5));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));

            //2nd page
            IActionResult actionResult2 = publishersController.GetAllPublishers("name_desc", "Pub", 2);
            Assert.That(actionResult2, Is.TypeOf<OkObjectResult>());
            var actionResultData2 = (actionResult2 as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData2.Count, Is.EqualTo(1));
            Assert.That(actionResultData2.First().Id, Is.EqualTo(1));
            Assert.That(actionResultData2.First().Name, Is.EqualTo("Publisher 1"));
        }

        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_ReturnOk()
        {
            IActionResult actionResult = publishersController.GetPublisherById(1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as Publisher;
            Assert.That(actionResultData.Id, Is.EqualTo(1));
            Assert.That(actionResultData.Name, Is.EqualTo("publisher 1").IgnoreCase);
        }

        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_NotFound()
        {
            IActionResult actionResult = publishersController.GetPublisherById(88);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnCreated_Test()
        {
            var newPublisher = new PublisherVM()
            {
                Name = "Publisher 7"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisher);
            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
            var actionResultData = (actionResult as CreatedResult).Value as Publisher;

            Assert.That(actionResultData.Id, Is.EqualTo(7));
            Assert.That(actionResultData.Name, Is.EqualTo("Publisher 7"));
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnBadRequest_Test()
        {
            var newPublisher = new PublisherVM()
            {
                Name = "7 pub"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisher);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(6)]
        public void HTTPDELETE_DeletePublisherById_ReturnOk_Test()
        {
            IActionResult actionResult = publishersController.DeletePublisherById(6);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HTTPDELETE_DeletePublisherById_ReturnBadRequest_Test()
        {
            IActionResult actionResult = publishersController.DeletePublisherById(88);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
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

            context.SaveChanges();
        }
    }
}
