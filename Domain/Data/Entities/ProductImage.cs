using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Domain.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Guid? ProductId { get; set; }
        public int? ProductVariationId { get; set; }
        public string ImageUrl { get; set; }
        public Product Product { get; set; }
        //public ProductVariation? ProductVariation { get; set; }
    }
}
