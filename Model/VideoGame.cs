using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend_VideoGamesCatalogue.Model
{
    public class VideoGame
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(50),]
        public string Platform { get; set; } = string.Empty;

        [Required, Precision(18,2)]
        public decimal Price { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        [Required, MaxLength(500)]
        public string ImageUrl { get; set; }

        [Required, MaxLength(100)]
        public string Genre { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
