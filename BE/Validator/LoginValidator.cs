using BE.DTOs;
using FluentValidation;

public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email hoặc Username không được để trống.")
            .Length(3, 20).WithMessage("Email hoặc Username phải từ 3 đến 20 ký tự.")
            .Matches(@"^(^[a-zA-Z0-9_]{3,20}$|^[^@\s]+@[^@\s]+\.[^@\s]+$)$")
            .WithMessage("Email hoặc Username không hợp lệ.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống.")
            .MinimumLength(8).WithMessage("Password phải có ít nhất 8 ký tự.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password không hợp lệ.");
    }
}