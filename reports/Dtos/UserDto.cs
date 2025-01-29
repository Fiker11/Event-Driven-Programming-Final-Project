using System.ComponentModel.DataAnnotations;

namespace Reports.Dtos
{
    public class UserDto
    {
        [Required]
        public string? UserName { get; set; } //name of the user

        [Required]
        [EmailAddress]

        public string? UserEmail { get; set; } //email of the user

        [Required]

        public string? UserPhoneNumber { get; set; } //phone number of the user

        [Required]

        public string? UserAddress { get; set; } //address of the user

        //date and time of the user creation is removed because the user doesnt provide the date and time
    }
}