using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
