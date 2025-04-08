using Domain.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Data.Entities
{
    public class Category : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }

        [JsonIgnore]
        public Category? ParentCategory { get; set; }

        [JsonIgnore]
        public ICollection<Category>? ChildCategories { get; set; }

        public ICollection<Product>? Products { get; set; }
        public ICollection<CategoryAttributes>? CategoryAttributes { get; set; }
    }
}