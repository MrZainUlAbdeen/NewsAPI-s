using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBook.Models
{
    public class User : TrackableBaseEntity
    {
        
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public Role Role { get; set; }
        
        public virtual ICollection<News> News { get; set; }
        
        public virtual ICollection<FavouriteNews> FavouriteNews { get; set; }
        
    }
}
