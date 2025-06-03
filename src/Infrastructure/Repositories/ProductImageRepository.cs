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
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductImageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(Guid productId, List<string> imageUrls, CancellationToken cancellationToken)
        {
            if (imageUrls == null || !imageUrls.Any())
            {
                return;
            }

            var images = imageUrls.Select(url => new ProductImage
            {
                ProductId = productId,
                ImageName = url.Split('/').Last().Split('?')[0],
                ImageUrl = url
            }).ToList();

            await _dbContext.ProductImages.AddRangeAsync(images, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var images = await _dbContext.ProductImages
                .Where(img => img.ProductId == productId)
                .ToListAsync(cancellationToken);

            if (images.Any())
            {
                _dbContext.ProductImages.RemoveRange(images);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
