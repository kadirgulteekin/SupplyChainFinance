using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? TaxId { get; set; } 

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? PhoneNumber { get; set; } 

        [StringLength(100)]
        public string? Email { get; set; } 
    }
}
