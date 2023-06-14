using Microsoft.AspNetCore.Mvc;
using MyBookList.Database;
using MyBookList.Models;

namespace MyBookList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FillController : ControllerBase
    {
        private BookRepository bookRepository;
        private UserRepository userRepository;

        public FillController()
        {
            string connectionString = "mongodb://localhost:27017/";
            string databaseName = "MyBookList";
            bookRepository = new BookRepository(connectionString, databaseName);
            userRepository = new UserRepository(connectionString, databaseName);
        }

        [HttpGet]
        public IActionResult FillData()
        {
            // Create User objects
            var users = new[]
            {
                new User
                {
                    Id = "614d64e7e6f31e127443ab01",
                    Username = "john",
                    Password = "password123",
                    BookInfos = new List<UserBookInfo>
                    {
                        new UserBookInfo
                        {
                            BookId = "614d64e7e6f31e127443ab02",
                            Status = "Reading",
                            PagesRead = 100,
                            ReReadCount = 0
                        },
                        new UserBookInfo
                        {
                            BookId = "614d64e7e6f31e127443ab03",
                            Status = "Read",
                            PagesRead = 300,
                            ReReadCount = 1
                        }
                    }
                },
                new User
                {
                    Id = "614d64e7e6f31e127443ab04",
                    Username = "emma",
                    Password = "password456",
                    BookInfos = new List<UserBookInfo>
                    {
                        new UserBookInfo
                        {
                            BookId = "614d64e7e6f31e127443ab02",
                            Status = "Read",
                            PagesRead = 250,
                            ReReadCount = 2
                        }
                    }
                }
            };

            // Create Book objects
            var books = new[]
            {
                new Book
                {
                    Id = "614d64e7e6f31e127443ab02",
                    Title = "Book 1",
                    Author = "Author 1",
                    Genre = "Genre 1"
                },
                new Book
                {
                    Id = "614d64e7e6f31e127443ab03",
                    Title = "Book 2",
                    Author = "Author 2",
                    Genre = "Genre 2"
                }
            };

            // Seed the database
            bookRepository.AddBook(books[0]);
            bookRepository.AddBook(books[1]);
            
            userRepository.AddUser(users[0]);
            userRepository.AddUser(users[1]);
            
            return Ok(users[0]);
        }
    }
}
