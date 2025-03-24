using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Brewery_DB_Service.Model
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        public TasteProfile? TasteProfile { get; set; }
    }
}