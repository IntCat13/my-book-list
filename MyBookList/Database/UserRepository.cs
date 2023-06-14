using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using MyBookList.Models;

namespace MyBookList.Database;

public class UserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _usersCollection = database.GetCollection<User>("users");
    }

    public async Task AddUser(User user)
    {
        user.Password = HashPassword(user.Password);
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task<User> GetUserById(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateUser(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        await _usersCollection.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteUser(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        await _usersCollection.DeleteOneAsync(filter);
    }

    public async Task<List<User>> GetAllUsers()
    {
        var filter = Builders<User>.Filter.Empty;
        return await _usersCollection.Find(filter).ToListAsync();
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync() != null;
    }

    public async Task<User> AuthenticateUser(string username, string password)
    {
        // Find the user with the provided login
        var filter = Builders<User>
            .Filter
            .Eq(u => u.Username, username);
        var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();

        if (user != null)
        {
            // Hash the provided password
            string hashedPassword = HashPassword(password);

            // Compare the hashed password with the stored password
            if (user.Password == hashedPassword)
                return user;
        }

        return null; // Authentication failed
    }
    
    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedPassword = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return hashedPassword;
        }
    }
    
    private User HashPassword(User user)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            user.Password = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return user;
        }
    }
}