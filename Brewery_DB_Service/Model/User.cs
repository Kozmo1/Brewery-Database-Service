using System.ComponentModel.DataAnnotations;

namespace Brewery_DB_Service.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public TasteProfile TasteProfile { get; set; } = new TasteProfile();
    }
}