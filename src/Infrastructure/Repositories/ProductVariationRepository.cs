using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductVariationRepository : IProductVariationRepository
    {
        private readonly AppDbContext _context;
        public ProductVariationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(List<ProductVariations> productVariations, CancellationToken cancellationToken)
        {
            if (productVariations == null || !productVariations.Any())
            {
                return;
            }

            await _context.ProductVariations.AddRangeAsync(productVariations, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
