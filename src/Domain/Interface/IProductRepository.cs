using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProductRepository
    {
        Task<int> CountProductByCategory(int categoryId, CancellationToken cancellationToken);
        Task RemoveCategoryFromProducts(int categoryId, CancellationToken cancellationToken);
        Task<Guid> AddAsync(Product product, CancellationToken cancellationToken);
    }
}
