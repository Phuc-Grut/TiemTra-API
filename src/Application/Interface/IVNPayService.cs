
using Domain.Data.Models.VNPAY;
using Microsoft.AspNetCore.Http;

namespace Application.Interface
{    
    public interface IVNPayService
    {
        void Initialize(
            string tmnCode,
            string hashSecret,
            string baseUrl,
            string callbackUrl,
            string version = "2.1.0",
            string orderType = "other");
        string GetPaymentUrl(PaymentRequest request);
        PaymentResult GetPaymentResult(IQueryCollection parameters);
    }
}
