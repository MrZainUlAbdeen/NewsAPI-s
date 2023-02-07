using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBook.Models
{
    public class News : TrackableBaseEntity
    {
        
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<FavouriteNews> FavouriteNews { get; set; }
    }

}
