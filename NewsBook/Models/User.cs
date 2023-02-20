using System.ComponentModel.DataAnnotations;

namespace NewsBook.Models
{
    public class User : TrackableBaseEntity
    {
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]

        public Role Role { get; set; }
        public virtual ICollection<Picture> Profile { get; set; }

        public virtual ICollection<News> News { get; set; }

        public virtual ICollection<FavouriteNews> FavouriteNews { get; set; }

    }
}
