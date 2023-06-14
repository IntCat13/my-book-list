using Microsoft.AspNetCore.Mvc;
using MyBookList.Database;
using MyBookList.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MyBookList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private BookRepository bookRepository;
        private UserRepository userRepository;

        public UsersController()
        {
            string connectionString =
                "mongodb://localhost:27017/";
            string databaseName = "MyBookList";
            bookRepository = new BookRepository(connectionString, databaseName);
            userRepository = new UserRepository(connectionString, databaseName);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await userRepository.AddUser(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser(string id)
        {
            try
            {
                await userRepository.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = userRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("books")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserBooks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var user = await userRepository.GetUserById(userId);
                if (user == null)
                    return NotFound();

                return Ok(user.BookInfos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var user = await userRepository.GetUserById(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("books")]
        [Authorize]
        public async Task<IActionResult> UpdateUserBook([FromBody] UpdatedUserBookData updatedBookData)
        {
            try
            {
                // Get current user
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                User currentUser = await userRepository.GetUserById(userId);
    
                // Check if user exists
                var userBook = currentUser.BookInfos.FirstOrDefault(b => b.BookId == updatedBookData.BookId);
                if (userBook == null)
                {
                    return NotFound();
                }

                currentUser.BookInfos.Remove(userBook);
                // Update book info
                userBook.Status = updatedBookData.Status;
                userBook.PagesRead = updatedBookData.PagesRead;
                userBook.ReReadCount = updatedBookData.ReReadCount;
                
                currentUser.BookInfos.Add(userBook);
                userRepository.UpdateUser(currentUser);

                // Save changes

                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        public class UpdatedUserBookData
        {
            public string BookId { get; set; }
            public string Status { get; set; }
            public int PagesRead { get; set; }
            public int ReReadCount { get; set; }
        }
        
        [HttpPost("handleBook")]
        [Authorize]
        public async Task<ActionResult<List<UserBookInfo>>> AddBookToList([FromBody] BookIdRequest request)
        {
            // Get current user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.BookInfos == null)
                user.BookInfos = new List<UserBookInfo>();
            
            // Check if book already exists in the user's list
            if (user.BookInfos.Any(b => b.BookId == request.BookId))
            {
                return BadRequest("Book already exists in the user's list");
            }

            // Get book by id
            Book book = await bookRepository.GetBookById(request.BookId);
            Console.WriteLine("Add book "+book.Title);
            if (book == null)
            {
                return NotFound();
            }

            // Add book to user's list
            user.BookInfos.Add(new UserBookInfo
            {
                BookId = book.Id,
                Status = "Not Read",
                PagesRead = 0,
                ReReadCount = 0
            });

            userRepository.UpdateUser(user);
            
            return Ok(user.BookInfos);
        }

        [HttpDelete("handleBook")]
        [Authorize]
        public async Task<ActionResult<List<UserBookInfo>>> RemoveBookFromList([FromBody] BookIdRequest request)
        {
            // Get current user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Check if book exists in the user's list
            UserBookInfo userBook = user.BookInfos.FirstOrDefault(b => b.BookId == request.BookId);

            if (userBook == null)
            {
                return BadRequest("Book does not exist in the user's list");
            }

            // Remove book from user's list
            user.BookInfos.Remove(userBook);

            userRepository.UpdateUser(user);

            return Ok(user.BookInfos);
        }
    }

    public class BookIdRequest
    {
        public string BookId { get; set; }
    }
}