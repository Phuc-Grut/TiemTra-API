using Domain.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities
{
    public class Attributes : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        public ICollection<CategoryAttributes> CategoryAttributes { get; set; }
    }
}