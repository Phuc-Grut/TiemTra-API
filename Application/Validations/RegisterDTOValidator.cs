using Application.DTOs.Authentication;
using FluentValidation;

namespace Application.Validations
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Vui lòng nhập Email.")
                .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Vui lòng nhập tên của bạn.")
                .MinimumLength(3).WithMessage("Tên đầy đủ phải có ít nhất 3 ký tự.")
                .MaximumLength(50).WithMessage("Tên đầy đủ không quá 50 ký tự.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Vui lòng nhập sđt.")
                .Matches(@"^[0-9]{10,11}").WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu và nhớ nó.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
                .MaximumLength(20).WithMessage("Mật khẩu không được quá 20 ký tự để dễ nhớ.");
        }
    }
}