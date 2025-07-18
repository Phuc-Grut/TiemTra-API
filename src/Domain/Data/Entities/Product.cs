﻿using Domain.Data.Base;
using Domain.Enum;

namespace Domain.Data.Entities
{
    public class Product : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? PrivewImage { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Origin { get; set; }
        public bool HasVariations { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? TotalSold { get; set; } = 0;
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Draft;
        public string? Note { get; set; }

        //public string?  { get; set; }
        public Category? Category { get; set; }

        public Brand? Brand { get; set; }

        public ICollection<ProductAttribute>? ProductAttributes { get; set; }
        public ICollection<ProductVariations>? ProductVariations { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }

        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public bool IsDeleted => ProductStatus == ProductStatus.Deleted;
    }
}