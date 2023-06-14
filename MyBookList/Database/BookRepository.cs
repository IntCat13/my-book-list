using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBookList.Models;

namespace MyBookList.Database
{
    public class BookRepository
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BookRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _booksCollection = database.GetCollection<Book>("books");
        }

        public async Task AddBook(Book book)
        {
            await _booksCollection.InsertOneAsync(book);
        }

        public async Task<Book> GetBookById(string id)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, id);
            return await _booksCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetAllBooks()
        {
            var filter = Builders<Book>.Filter.Empty;
            var books = await _booksCollection.Find(filter).ToListAsync();
            return books;
        }

        public async Task UpdateBook(Book book)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, book.Id);
            await _booksCollection.ReplaceOneAsync(filter, book);
        }

        public async Task DeleteBook(string id)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, id);
            await _booksCollection.DeleteOneAsync(filter);
        }
    }
}