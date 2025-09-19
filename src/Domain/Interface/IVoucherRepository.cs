using Domain.Data.Entities;
using Domain.DTOs;
using Domain.Enum;

namespace Domain.Interface
{
    public interface IVoucherRepository
    {
        Task<Voucher> CreateAsync(Voucher voucher, CancellationToken cancellationToken);

        Task<Voucher?> GetByIdAsync(Guid voucherId, CancellationToken cancellationToken);

        Task<Voucher?> GetByCodeAsync(string voucherCode, CancellationToken cancellationToken);

        Task<PagedResult<Voucher>> GetPagedAsync(int pageNumber, int pageSize, VoucherStatus? status, string? keyword, CancellationToken cancellationToken);

        Task<List<Voucher>> GetPublicVouchersAsync(CancellationToken cancellationToken);

        Task<bool> UpdateAsync(Voucher voucher, CancellationToken cancellationToken);

        Task<bool> UpdateStatusAsync(Guid voucherId, VoucherStatus status, Guid updatedBy, CancellationToken cancellationToken);

        Task<bool> VoucherCodeExistsAsync(string voucherCode, CancellationToken cancellationToken);

        Task<string> GenerateUniqueVoucherCodeAsync(CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        // Thêm methods xóa
        Task<bool> SoftDeleteAsync(Guid voucherId, Guid updatedBy, CancellationToken cancellationToken);
        Task<bool> HardDeleteAsync(Guid voucherId, CancellationToken cancellationToken);
        Task<bool> RestoreAsync(Guid voucherId, Guid updatedBy, CancellationToken cancellationToken);
        Task<bool> HasUsedVouchersAsync(Guid voucherId, CancellationToken cancellationToken);
        
        // Thêm method để lấy voucher hết hạn
        Task<List<Voucher>> GetExpiredVouchersAsync(CancellationToken cancellationToken);
    }
}