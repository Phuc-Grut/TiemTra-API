using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IAttributesRepository
    {
        Task<Attributes> AddAttribute(Attributes attributes , CancellationToken cancellationToken);
        Task<IEnumerable<Attributes>> GetAllAttributes(CancellationToken cancellationToken);
    }
}
