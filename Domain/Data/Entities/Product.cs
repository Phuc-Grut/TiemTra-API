using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class Product : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string ProductName { get; set; } 
        public string? Description { get; set; }
        public string? PrivewImage { get; set; }
        public string? Origin { get; set; }
        //public string?  { get; set; }
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}
