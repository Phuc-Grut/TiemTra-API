using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin.Brand
{
    public class UpdateBrandDTO
    {
        [Required]
        public int BrandId { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string? Logo { get; set; }
        public string? Description { get; set; }
    }
}