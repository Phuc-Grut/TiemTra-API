using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProductAttributeRepository
    {
        Task AddRangeAsync(Guid productId, List<ProductAttribute> attributes);
    }
}
