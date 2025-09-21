using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _dbContext;

        public BrandRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Brand?> AddBrandAsync(Brand brand, CancellationToken cancellationToken)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return brand;
        }

        public async Task<List<int>> DeleteBrandsAsync(List<int> brandIds, CancellationToken cancellationToken)
        {
            var brands = await _dbContext.Brands.Where(b => brandIds.Contains(b.BrandId)).ToListAsync(cancellationToken);
            _dbContext.Brands.RemoveRange(brands);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return brands.Select(b => b.BrandId).ToList();
        }

        public async Task<List<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Brands.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Brands.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> UpdateBrandAsync(Brand brand, CancellationToken cancellationToken)
        {
            _dbContext.Brands.Update(brand);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}