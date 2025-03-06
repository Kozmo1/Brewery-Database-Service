using System.ComponentModel.DataAnnotations;

namespace Brewery_DB_Service.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}