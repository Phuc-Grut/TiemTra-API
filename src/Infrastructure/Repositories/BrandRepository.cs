using Domain.Data.Entities;
using Domain.DTOs.Product;
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
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _dbContext;

        public BrandRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Brand> AddBrandAsync(Brand brand, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Brands.Add(brand);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return brand;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandRepository][Add] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteBrandAsync(int brandId, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _dbContext.Brands.FindAsync(new object[] { brandId }, cancellationToken);
                if (brand == null)
                    return false;

                _dbContext.Brands.Remove(brand);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandRepository][Delete] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<int>> DeleteBrandsAsync(List<int> brandIds, CancellationToken cancellationToken)
        {
            var deletedIds = new List<int>();

            var brandsToDelete = _dbContext.Brands.Where(b => brandIds.Contains(b.BrandId)).ToList();

            if (!brandsToDelete.Any())
                return deletedIds;

            _dbContext.Brands.RemoveRange(brandsToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);

            deletedIds.AddRange(brandsToDelete.Select(b => b.BrandId));
            return deletedIds;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Brands.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandRepository][GetAll] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Brand?> GetBrandByIdAsync(int brandId, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.Brands.FirstOrDefaultAsync(b => b.BrandId == brandId, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandRepository][GetById] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateBrandAsync(Brand brand, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Brands.Update(brand);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandRepository][Update] Error: {ex.Message}");
                throw;
            }
        }
    }
}
