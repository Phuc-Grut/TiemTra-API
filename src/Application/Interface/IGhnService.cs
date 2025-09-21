using Application.DTOs.GHN;

namespace Application.Interface
{
    public interface IGhnService
    {
        Task<GhnCalculateFeeResponse> CalculateFeeAsync(GhnCalculateFeeRequest request, CancellationToken ct);
    }
}