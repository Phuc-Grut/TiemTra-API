using Domain.Data.Entities;

namespace Domain.Interface
{
    public interface IBrandRepository
    {
        /// Admin
        Task<List<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken);

        Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken);

        Task<Brand?> AddBrandAsync(Brand brand, CancellationToken cancellationToken);

        Task<bool> UpdateBrandAsync(Brand brand, CancellationToken cancellationToken);

        Task<List<int>> DeleteBrandsAsync(List<int> brandIds, CancellationToken cancellationToken);

        /// Store
    }
}