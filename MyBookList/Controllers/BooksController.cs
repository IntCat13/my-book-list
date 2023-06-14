using Microsoft.AspNetCore.Mvc;
using MyBookList.Database;
using MyBookList.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private BookRepository bookRepository;

        public BooksController()
        {
            string connectionString = "mongodb://localhost:27017/";
            string databaseName = "MyBookList";
            bookRepository = new BookRepository(connectionString, databaseName);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await bookRepository.GetAllBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            try
            {
                var book = await bookRepository.GetBookById(id);
                if (book == null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await bookRepository.AddBook(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingBook = await bookRepository.GetBookById(id);
                if (existingBook == null)
                    return NotFound();

                book.Id = existingBook.Id;
                await bookRepository.UpdateBook(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                var existingBook = await bookRepository.GetBookById(id);
                if (existingBook == null)
                    return NotFound();

                await bookRepository.DeleteBook(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
