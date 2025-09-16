using Application.DTOs.GHN;
using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GhnService : IGhnService
    {
        private readonly HttpClient _http;
        public GhnService(HttpClient http)
        {
            _http = http;
        }

        public async Task<GhnCalculateFeeResponse> CalculateFeeAsync(GhnCalculateFeeRequest request, CancellationToken ct)
        {
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
