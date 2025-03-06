using System.ComponentModel.DataAnnotations;

namespace Brewery_DB_Service.Model
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}