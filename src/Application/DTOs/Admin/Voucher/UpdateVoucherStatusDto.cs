using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.DTOs.Admin.Voucher
{
    public class UpdateVoucherStatusDto
    {
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public VoucherStatus Status { get; set; }
    }
}