

using Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class VoucherExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VoucherExpirationService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(1); // Chạy mỗi s

        public VoucherExpirationService(IServiceProvider serviceProvider, ILogger<VoucherExpirationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var voucherService = scope.ServiceProvider.GetRequiredService<IVoucherService >();
                    
                    var updatedCount = await voucherService.UpdateExpiredVouchersAsync(stoppingToken);
                    
                    if (updatedCount > 0)
                    {
                        _logger.LogInformation($"Đã cập nhật {updatedCount} voucher hết hạn sang trạng thái OutDate");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi cập nhật voucher hết hạn");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }
    }
} 