
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
    }
}