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
                await _userCollection.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create user.", ex);
            }
        }

        //get all users
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

        //get user by id
        public async Task<User> GetUserById(string id)
        {
            //check if the user exists
            var user = await _userCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            try
            {
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve user.", ex);
            }
        }

        //update user by id 
        public async Task UpdateUser(string id, User user)
        {
            var existingUser = await GetUserById(id);
            user.UserId = id; //so that the id is not changed
            try
            {
                //update the user
                var result = await _userCollection.ReplaceOneAsync(user => user.UserId == id, user);

                //if the user is not updated, throw an exception
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

        //get user by email
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userCollection.Find(user => user.UserEmail == email).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with email {email} not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve user.", ex);
            }
        }

        //Delete user by id
        public async Task DeleteUser(string id)
        {
            //check if the user exists
            var existingUser = await GetUserById(id);

            try
            {
                //if the user exists, delete the user
                var result = await _userCollection.DeleteOneAsync(user => user.UserId == id);

                //if the user is not deleted, throw an exception
                if (result.DeletedCount == 0)
                {
                    throw new Exception($"Failed to delete the user with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete user.", ex);
            }
        }

        //get users paginated
        public Task<List<User>> GetUsersPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }
            try
            {
                return _userCollection.Find(user => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve users.", ex);
            }
        }

        //search users by name and email
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