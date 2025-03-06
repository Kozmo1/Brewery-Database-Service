using System.ComponentModel.DataAnnotations;

namespace Brewery_DB_Service.Model
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public float ReviewRating { get; set; }
        [Required]
        public string ReviewMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}