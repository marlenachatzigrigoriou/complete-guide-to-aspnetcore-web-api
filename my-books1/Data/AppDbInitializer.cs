﻿using my_books1.Data.Models;

namespace my_books1.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Book.Any())
                {
                    context.Book.AddRange(new Book()
                    {
                        Title = "1st Book Title",
                        Description = "1st Book Description",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-10),
                        Rate = 4,
                        Genre = "Biography",
                        Author = "Me",
                        CoverUrl = "https....",
                        DateAdded = DateTime.Now
                    },
                    new Book()
                    {
                        Title = "2nd Book Title",
                        Description = "2nd Book Description",
                        IsRead = false,
                        Genre = "Biography",
                        Author = "Me",
                        CoverUrl = "https....",
                        DateAdded = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
        }

    }
}