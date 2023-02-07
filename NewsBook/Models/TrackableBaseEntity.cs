using EntityFrameworkCore.Triggers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsBook.Models
{
    public abstract class TrackableBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        static TrackableBaseEntity()
        {
            Triggers<TrackableBaseEntity>.Inserting += entry => entry.Entity.Id = Guid.NewGuid();
            Triggers<TrackableBaseEntity>.Inserting += entry => entry.Entity.CreatedAt = entry.Entity.UpdatedAt = DateTime.Now;
            Triggers<TrackableBaseEntity>.Updating  += entry => entry.Entity.UpdatedAt = DateTime.Now;
        }
    }
}
