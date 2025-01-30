using MongoDB.Driver;
using Reports.Models;

namespace Reports.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("MongoDbSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDbSettings:DatabaseName"));
            _userCollection = database.GetCollection<User>(configuration.GetValue<string>("MongoDbSettings:UserCollectionName"));
        }

        //create user
        public async Task CreateUser(User user)
        {
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                await _userCollection.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create user.", ex);
            }
        }

        //Delete user
        public async Task DeleteUser(string id)
        {
            var existingUser = await _userCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            var result = await _userCollection.DeleteOneAsync(user => user.UserId == id);
            if (result.DeletedCount == 0)
            {
                throw new Exception($"Failed to delete the user with ID {id}.");
            }
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userCollection.Find(user => user.UserEmail == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found.");
            }
            return user;
        }

        public Task<List<User>> GetUsers()
        {
            try
            {
                return _userCollection.Find(user => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve users.", ex);
            }
        }

        public async Task UpdateUser(string id, User user)
        {
            try
            {
                var existingUser = await _userCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();
                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }
                var result = await _userCollection.ReplaceOneAsync(user => user.UserId == id, user);
                if (result.ModifiedCount == 0)
                {
                    throw new Exception($"Failed to update the user with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update user.", ex);
            }
        }

        public Task<List<User>> GetUsersPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }
            try
            {
                return _userCollection.Find(user => true).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve users.", ex);
            }
        }

        public Task<List<User>> SearchUsers(string? name, string? email)
        {
            try
            {
                var filter = Builders<User>.Filter.Empty;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    filter &= Builders<User>.Filter.Regex(user => user.UserName, new MongoDB.Bson.BsonRegularExpression(name, "i"));
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filter &= Builders<User>.Filter.Regex(user => user.UserEmail, new MongoDB.Bson.BsonRegularExpression(email, "i"));
                }

                return _userCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to search users.", ex);
            }
        }
    }
}