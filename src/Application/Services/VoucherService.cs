using Application.DTOs;
using Application.DTOs.Admin.Voucher;
using Application.DTOs.Store.Voucher;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs.Admin.Voucher;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUserRepository _userRepository;

        public VoucherService(IVoucherRepository voucherRepository, IUserRepository userRepository)
        {
            _voucherRepository = voucherRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse> CreateVoucherAsync(CreateVoucherDto dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                if (dto.EndDate <= DateTime.UtcNow)
                {
                    return new ApiResponse(false, "Ngày hết hạn phải sau thời điểm hiện tại");
                }

                var voucherCode = await _voucherRepository.GenerateUniqueVoucherCodeAsync(cancellationToken);

                var voucher = new Voucher
                {
                    VoucherId = Guid.NewGuid(),
                    VoucherCode = voucherCode,
                    VoucherName = dto.VoucherName.Trim(),
                    Description = dto.Description?.Trim(),
                    Quantity = dto.Quantity,
                    DiscountPercentage = dto.DiscountPercentage,
                    EndDate = dto.EndDate,
                    Status = VoucherStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    UpdatedBy = userId
                };

                var result = await _voucherRepository.CreateAsync(voucher, cancellationToken);

                return new ApiResponse(true, "Tạo voucher thành công", new
                {
                    voucherId = result.VoucherId,
                    voucherCode = result.VoucherCode,
                    voucherName = result.VoucherName
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi tạo voucher: {ex.Message}");
            }
        }

        public async Task<PagedResult<VoucherDto>> GetPagedVouchersAsync(int pageNumber, int pageSize, VoucherStatus? status, string? keyword, CancellationToken cancellationToken)
        {
            var result = await _voucherRepository.GetPagedAsync(pageNumber, pageSize, status, keyword, cancellationToken);

            var voucherDtos = result.Items.Select(v => new VoucherDto
            {
                VoucherId = v.VoucherId,
                VoucherCode = v.VoucherCode,
                VoucherName = v.VoucherName,
                Description = v.Description,
                Quantity = v.Quantity,
                UsedQuantity = v.UsedQuantity,
                DiscountPercentage = v.DiscountPercentage,
                EndDate = v.EndDate,
                Status = v.Status,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                CreatorName = v.Creator?.FullName,
                UpdaterName = v.Updater?.FullName
            }).ToList();

            return new PagedResult<VoucherDto>
            {
                Items = voucherDtos,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize
            };
        }

        public async Task<VoucherDto?> GetVoucherByIdAsync(Guid voucherId, CancellationToken cancellationToken)
        {
            var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
            if (voucher == null) return null;

            return new VoucherDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCode = voucher.VoucherCode,
                VoucherName = voucher.VoucherName,
                Description = voucher.Description,
                Quantity = voucher.Quantity,
                UsedQuantity = voucher.UsedQuantity,
                DiscountPercentage = voucher.DiscountPercentage,
                EndDate = voucher.EndDate,
                Status = voucher.Status,
                CreatedAt = voucher.CreatedAt,
                UpdatedAt = voucher.UpdatedAt,
                CreatorName = voucher.Creator?.FullName,
                UpdaterName = voucher.Updater?.FullName
            };
        }

        public async Task<ApiResponse> UpdateVoucherStatusAsync(Guid voucherId, VoucherStatus status, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    return new ApiResponse(false, "Không tìm thấy voucher");
                }

                if (status == VoucherStatus.Publish && voucher.EndDate <= DateTime.UtcNow)
                {
                    return new ApiResponse(false, "Không thể công khai voucher đã hết hạn");
                }

                var result = await _voucherRepository.UpdateStatusAsync(voucherId, status, userId, cancellationToken);

                if (!result)
                {
                    return new ApiResponse(false, "Cập nhật trạng thái thất bại");
                }

                var statusDisplay = status switch
                {
                    VoucherStatus.Pending => "Chờ phê duyệt",
                    VoucherStatus.Publish => "Công khai",
                    VoucherStatus.Deleted => "Đã xóa",
                    _ => "Không xác định"
                };

                return new ApiResponse(true, $"Cập nhật trạng thái thành '{statusDisplay}' thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi cập nhật trạng thái: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateVoucherAsync(Guid voucherId, CreateVoucherDto dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    return new ApiResponse(false, "Không tìm thấy voucher");
                }

                voucher.VoucherName = dto.VoucherName.Trim();
                voucher.Description = dto.Description?.Trim();
                voucher.Quantity = dto.Quantity;
                voucher.DiscountPercentage = dto.DiscountPercentage;
                voucher.EndDate = dto.EndDate;
                voucher.UpdatedAt = DateTime.UtcNow;
                voucher.UpdatedBy = userId;
                voucher.Status = dto.VoucherStatus; 

                var result = await _voucherRepository.UpdateAsync(voucher, cancellationToken);

                if (!result)
                {
                    return new ApiResponse(false, "Cập nhật voucher thất bại");
                }

                return new ApiResponse(true, "Cập nhật voucher thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi cập nhật voucher: {ex.Message}");
            }
        }

        public async Task<List<PublicVoucherDto>> GetPublicVouchersAsync(CancellationToken cancellationToken)
        {
            var vouchers = await _voucherRepository.GetPublicVouchersAsync(cancellationToken);

            return vouchers.Select(v => new PublicVoucherDto
            {
                VoucherId = v.VoucherId,
                VoucherCode = v.VoucherCode,
                VoucherName = v.VoucherName,
                Description = v.Description,
                DiscountPercentage = v.DiscountPercentage,
                EndDate = v.EndDate,
                RemainingQuantity = v.Quantity - v.UsedQuantity
            }).ToList();
        }

        public async Task<string> GenerateVoucherCodeAsync(CancellationToken cancellationToken)
        {
            return await _voucherRepository.GenerateUniqueVoucherCodeAsync(cancellationToken);
        }

        public async Task<ApplyVoucherResponse> ApplyVoucherAsync(ApplyVoucherRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //Tìm voucher theo mã
                var voucher = await _voucherRepository.GetByCodeAsync(request.VoucherCode, cancellationToken);

                if (voucher == null)
                {
                    return new ApplyVoucherResponse
                    {
                        IsValid = false,
                        Message = "Mã voucher không tồn tại",
                        DiscountAmount = 0,
                        FinalAmount = request.OrderTotal,
                    };
                }

                //Kiểm tra trạng thái voucher
                if (voucher.Status != VoucherStatus.Publish)
                {
                    return new ApplyVoucherResponse
                    {
                        IsValid = false,
                        Message = "Voucher chưa được kích hoạt",
                        DiscountAmount = 0,
                        FinalAmount = request.OrderTotal
                    };
                }

                //Kiểm tra ngày hết hạn
                if (voucher.EndDate <= DateTime.UtcNow)
                {
                    return new ApplyVoucherResponse
                    {
                        IsValid = false,
                        Message = "Voucher đã hết hạn",
                        DiscountAmount = 0,
                        FinalAmount = request.OrderTotal
                    };
                }

                //Kiểm tra số lượng còn lại
                if (voucher.UsedQuantity >= voucher.Quantity)
                {
                    return new ApplyVoucherResponse
                    {
                        IsValid = false,
                        Message = "Voucher đã hết lượt sử dụng",
                        DiscountAmount = 0,
                        FinalAmount = request.OrderTotal
                    };
                }

                //Tính toán số tiền giảm giá
                var discountAmount = (request.OrderTotal * voucher.DiscountPercentage) / 100;
                var finalAmount = request.OrderTotal - discountAmount;

                //Đàm bảo số tiền cuối cùng không âm;
                if (finalAmount < 0)
                {
                    finalAmount = 0;
                    discountAmount = request.OrderTotal;
                }

                return new ApplyVoucherResponse
                {
                    IsValid = true,
                    Message = "Áp dụng voucher thành công",
                    DiscountAmount = discountAmount,
                    FinalAmount = finalAmount,
                    VoucherCode = voucher.VoucherCode,
                    DiscountPercentage = voucher.DiscountPercentage,
                    VoucherId = voucher.VoucherId,
                };
            }
            catch (System.Exception ex)
            {
                return new ApplyVoucherResponse
                {
                    IsValid = false,
                    Message = $"Lỗi khi áp dụng voucher :{ex.Message}",
                    DiscountAmount = 0,
                    FinalAmount = request.OrderTotal
                };
            }
        }

        public async Task<ApiResponse> UnpublishVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);
                var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    return new ApiResponse(false, "Không tìm thấy voucher");
                }
                if (voucher.Status != VoucherStatus.Publish)
                {
                    return new ApiResponse(false, "Voucher chưa được công khai, không thể hủy công khai");
                }


                //Kiểm tra xem voucher đã được sử dụng chưa
                if (voucher.UsedQuantity > 0)
                {
                    return new ApiResponse(false, "Không thể hủy công khai voucher đã có người sử dụng");
                }


                //Cập nhật trạng thái về Pending
                var result = await _voucherRepository.UpdateStatusAsync(voucherId, VoucherStatus.Pending, userId, cancellationToken);
                if (!result)
                {
                    return new ApiResponse(false, "Hủy công khai voucher thất bại");
                }
                return new ApiResponse(true, $"Hủy công khai voucher thành công");
            }
            catch (System.Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi hủy công khai voucher: {ex.Message}");
            }
        }

        public async Task<ApiResponse> HardDeleteVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    return new ApiResponse(false, "Không tìm thấy voucher");
                }

                // Kiểm tra xem voucher có đang được sử dụng trong đơn hàng không
                // var hasUsedVouchers = await _voucherRepository.HasUsedVouchersAsync(voucherId, cancellationToken);
                // if (hasUsedVouchers)
                // {
                //     return new ApiResponse(false, "Không thể xóa cứng voucher đã được sử dụng trong đơn hàng");
                // }

                var result = await _voucherRepository.HardDeleteAsync(voucherId, cancellationToken);
                if (!result)
                {
                    return new ApiResponse(false, "Xóa cứng voucher thất bại");
                }

                return new ApiResponse(true, "Xóa cứng voucher thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi xóa cứng voucher: {ex.Message}");
            }
        }

        public async Task<ApiResponse> RestoreVoucherAsync(Guid voucherId, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    return new ApiResponse(false, "Không tìm thấy voucher");
                }

                if (voucher.Status != VoucherStatus.Deleted)
                {
                    return new ApiResponse(false, "Voucher không ở trạng thái đã xóa");
                }

                var result = await _voucherRepository.RestoreAsync(voucherId, userId, cancellationToken);
                if (!result)
                {
                    return new ApiResponse(false, "Khôi phục voucher thất bại");
                }

                return new ApiResponse(true, "Khôi phục voucher thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Lỗi khi khôi phục voucher: {ex.Message}");
            }
        }

        public async Task<int> UpdateExpiredVouchersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var expiredVouchers = await _voucherRepository.GetExpiredVouchersAsync(cancellationToken);

                if (!expiredVouchers.Any())
                {
                    return 0;
                }

                var updatedCount = 0;
                foreach (var voucher in expiredVouchers)
                {
                    var result = await _voucherRepository.UpdateStatusAsync(voucher.VoucherId, VoucherStatus.OutDate, voucher.UpdatedBy, cancellationToken);
                    if (result)
                    {
                        updatedCount++;
                    }
                }

                return updatedCount;
            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid stopping the background service
                return 0;
            }
        }

        public async Task<int> SoftDeleteVoucherAsync(IEnumerable<Guid> voucherIds, ClaimsPrincipal user, CancellationToken ct)
        {
            var ids = voucherIds.Where(x => x != Guid.Empty).Distinct().ToList() ?? new();

            if (ids.Count == 0)
            {
                return 0;
            }

            var userId = GetUserIdFromClaims.GetUserId(user);
            Guid updateBy = userId == Guid.Empty ? Guid.NewGuid() : userId;

            var utcNow = DateTime.UtcNow;

            return await _voucherRepository.SoftDeleteByIdsAsync(ids, updateBy, utcNow, ct);
        }
    }

}