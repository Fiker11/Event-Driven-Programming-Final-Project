using Microsoft.AspNetCore.Mvc;
using Reports.Services;
using Reports.Dtos;

namespace Reports.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //create a user 
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
        {   
            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create a new user
            var user = new User
            {
                UserName = userDto.UserName,
                UserEmail = userDto.UserEmail,
                UserPhoneNumber = userDto.UserPhoneNumber,
                UserAddress = userDto.UserAddress,
                CreatedAt = DateTime.UtcNow //set the created at date
            };

            //try to create the user or else catch the exception
            try
            {
                await _userService.CreateUser(user);
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get user by id
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(string userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                return Ok(user);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //update user by id
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
        {
            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //create a new user
            var user = new User
            {
                UserId = userId,
                UserName = userDto.UserName,
                UserEmail = userDto.UserEmail,
                UserPhoneNumber = userDto.UserPhoneNumber,
                UserAddress = userDto.UserAddress
            };

            //try to update the user or else catch the exception
            try
            {
                await _userService.UpdateUser(userId, user);
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get user by email
        [HttpGet("Email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //delete user by id
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            try
            {
                await _userService.DeleteUser(userId);
                return Ok("User deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get all users paginated
        [HttpGet("Paginated")]
        public async Task<ActionResult> GetUsersPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetUsersPaginated(pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //search users by name or email
        [HttpGet("Search")]
        public async Task<ActionResult> SearchUsers(
            [FromQuery] string? name, 
            [FromQuery] string? email)
        {
            try
            {
                var users = await _userService.SearchUsers(name, email);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}