using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class Category : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }

        [JsonIgnore]
        public Category? ParentCategory { get; set; }
        [JsonIgnore]
        public ICollection<Category>? ChildCategories { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<CategoryAttributes>? CategoryAttributes { get; set; }
    }
}
