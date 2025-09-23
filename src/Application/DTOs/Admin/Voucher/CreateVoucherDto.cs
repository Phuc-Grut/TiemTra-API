using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Admin.Voucher;

public class CreateVoucherDto
{
    [Required(ErrorMessage = "Tên voucher không được để trống")]
    [StringLength(100, ErrorMessage = "Tên voucher không được quá 100 ký tự")]
    public string VoucherName { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Phần trăm giảm giá không được để trống")]
    [Range(0.01, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0.01% đến 100%")]
    public decimal DiscountPercentage { get; set; }

    [Required(ErrorMessage = "Ngày hết hạn không được để trống")]
    public DateTime EndDate { get; set; }
}