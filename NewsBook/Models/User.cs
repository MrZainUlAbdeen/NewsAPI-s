using System.ComponentModel.DataAnnotations;

namespace NewsBook.Models
{
    public class User : TrackableBaseEntity
    {
        //public Guid Id { get; set; }
        //[Required (ErrorMessage ="Name is required")]
        //[StringLength(10 , ErrorMessage =("{10} Max lenght"))]
        public string Name { get; set; }
        //[EmailAddress (ErrorMessage = ("Incorrect Emal format"))]
        //[Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}
