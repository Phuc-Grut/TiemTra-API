using Domain.Data.Entities;
using Domain.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
