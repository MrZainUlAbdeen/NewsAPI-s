using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBook.Models
{
    public class FavouriteNews : TrackableBaseEntity
    {
        
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid NewsId { get; set; }
        public virtual News News { get; set; }
        public bool IsFavorite { get; set; }
    }
}
