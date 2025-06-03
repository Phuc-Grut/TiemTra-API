using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProductImageRepository
    {
        Task AddRangeAsync(Guid productId, List<string> imageUrls, CancellationToken cancellationToken);
        Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
