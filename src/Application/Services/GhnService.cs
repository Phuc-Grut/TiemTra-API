using Application.DTOs.GHN;
using Application.Interface;
using Microsoft.Extensions.Options;
using Shared.Common;
using System.Net.Http.Json;

namespace Application.Services
{
    public class GhnService : IGhnService
    {
        private readonly HttpClient _http;
        private readonly GhnOptions _options;

        public GhnService(HttpClient http, IOptions<GhnOptions> options)
        {
            _http = http;
            _options = options.Value; // lấy config từ appsettings
        }

        public async Task<GhnCalculateFeeResponse> CalculateFeeAsync(GhnCalculateFeeRequest request, CancellationToken ct)
        {
            request.FromDistrictId = _options.FromDistrictId;
            request.FromWardCode = _options.FromWardCode;

            using var httpRes = await _http.PostAsJsonAsync("shipping-order/fee", request, ct);
            var payload = await httpRes.Content.ReadFromJsonAsync<GhnEnvelope<GhnCalculateFeeResponse>>(cancellationToken: ct);

            if (payload is null)
                throw new InvalidOperationException("Không đọc được phản hồi từ GHN.");

            if (!httpRes.IsSuccessStatusCode || payload.Code != 200)
            {
                var message = payload.Message ?? $"GHN error {payload.Code}";
                throw new HttpRequestException(message);
            }

            return payload.Data!;
        }
    }
}
