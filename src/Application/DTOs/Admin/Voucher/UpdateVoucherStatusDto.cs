using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin.Voucher
{
    public class UpdateVoucherStatusDto
    {
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public VoucherStatus Status { get; set; }
    }
}