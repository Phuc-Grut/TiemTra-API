using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProductVariationRepository
    {
        Task AddRangeAsync(List<ProductVariations> productVariations, CancellationToken cancellationToken);
    }
}
