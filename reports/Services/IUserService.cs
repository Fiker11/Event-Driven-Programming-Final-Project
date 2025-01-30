namespace Reports.Services
{
    public interface IUserService
    {
        //Method to create a user
        public Task CreateUser(User user);

        //Method to get all users
        public Task<List<User>> GetUsers();

        //Method to get a user by id
        public Task<User> GetUserById(string id);

        //Method to get a user by email
        public Task<User> GetUserByEmail(string email);

        //Method to update a user by id
        public Task UpdateUser(string id, User user);

        //Method to remove a user by id
        public Task DeleteUser(string id);

        //Method to get users paginated
        Task<List<User>> GetUsersPaginated(int pageNumber, int pageSize);
        
        // Method to search users by name and email
        Task<List<User>> SearchUsers(string? name, string? email);
    }
}