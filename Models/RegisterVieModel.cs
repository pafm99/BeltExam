using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string PasswordConf { get; set; }
    }
}