using Application.DTOs;
using Application.DTOs.Admin.Voucher;
using Application.DTOs.Store.Voucher;
using Domain.DTOs.Admin.Voucher;
using Domain.Enum;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IVoucherService
    {
        Task<ApiResponse> CreateVoucherAsync(CreateVoucherDto dto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<PagedResult<VoucherDto>> GetPagedVouchersAsync(int pageNumber, int pageSize, VoucherStatus? status, string? keyword, CancellationToken cancellationToken);
        Task<VoucherDto?> GetVoucherByIdAsync(Guid voucherId, CancellationToken cancellationToken);
        Task<ApiResponse> UpdateVoucherStatusAsync(Guid voucherId, VoucherStatus status, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ApiResponse> UpdateVoucherAsync(Guid voucherId, CreateVoucherDto dto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<List<PublicVoucherDto>> GetPublicVouchersAsync(CancellationToken cancellationToken);
        Task<string> GenerateVoucherCodeAsync(CancellationToken cancellationToken);

        Task<ApplyVoucherResponse> ApplyVoucherAsync(ApplyVoucherRequest request, CancellationToken cancellationToken);

        Task<ApiResponse> UnpublishVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ApiResponse> SoftDeleteVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ApiResponse> HardDeleteVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ApiResponse> RestoreVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken);
        
        //method để cập nhật trạng thái voucher hết hạn
        Task<int> UpdateExpiredVouchersAsync(CancellationToken cancellationToken);
    }
}