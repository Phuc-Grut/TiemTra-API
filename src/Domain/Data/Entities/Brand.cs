using Domain.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities
{
    public class Brand : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BrandId { get; set; }

        public string BrandName { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}