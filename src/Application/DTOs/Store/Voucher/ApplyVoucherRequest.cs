using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Store.Voucher;

public class ApplyVoucherRequest {
    [Required(ErrorMessage = "Mã voucher không được để trống")]
    public string VoucherCode {get; set;}

    [Required(ErrorMessage="Tổng tiền đơn hàng không được trống")]
    [Range(0.01, double.MaxValue, ErrorMessage ="Tổng tiền đơn hàng phải lớn hơn 0")]
    public decimal OrderTotal {get; set;}
}