using Application.DTOs.GHN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IGhnService
    {
        Task<GhnCalculateFeeResponse> CalculateFeeAsync(GhnCalculateFeeRequest request, CancellationToken ct);
    }
}
