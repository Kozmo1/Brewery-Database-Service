using System.ComponentModel.DataAnnotations;

namespace Brewery_DB_Service.Model
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        public float ABV { get; set; }
        public float Volume { get; set; }
        public string Package { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderPoint { get; set; }
        public TasteProfile TasteProfile { get; set; } = new TasteProfile();
    }
}