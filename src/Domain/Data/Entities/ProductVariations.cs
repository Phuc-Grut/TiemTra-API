﻿using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class ProductVariations : BaseEntity
    {
        public Guid ProductVariationId { get; set; }
        public Guid ProductId { get; set; }

        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }

        public Product Product { get; set; }

        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();

        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }
}