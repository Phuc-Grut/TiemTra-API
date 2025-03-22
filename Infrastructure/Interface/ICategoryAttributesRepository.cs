using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategoryAttributesRepository
    {
        Task AddAttributesToCategory(CategoryAttributes categoryAttribute, CancellationToken cancellationToken);
        Task RemoveAttributesToCategory(CategoryAttributes categoryAttribute, CancellationToken cancellationToken);
        Task<List<Attributes>> GetAttributesByCategory(int categoryId);
        Task<bool> ExistsAsync(int categoryId, int attributeId);
    }
}
