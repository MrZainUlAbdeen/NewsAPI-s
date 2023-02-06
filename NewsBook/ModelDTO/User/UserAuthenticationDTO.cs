using System.ComponentModel.DataAnnotations;

namespace NewsBook.ModelDTO.User
{
    public class UserAuthenticationDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
