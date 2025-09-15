namespace Application.DTOs.Store.Voucher;

public class ApplyVoucherResponse{
    public bool IsValid {get; set;}
    
    public string Message {get; set;}

    public decimal DiscountAmount {get; set;}

    public decimal FinalAmount {get; set;}

    public string? VoucherCode {get; set;}
    
    public decimal? DiscountPercentage {get; set;}
    public Guid? VoucherId {get; set;}
}