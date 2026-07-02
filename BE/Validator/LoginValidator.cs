using BE.DTOs;
using FluentValidation;
using FluentValidation.Results;
public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email hoặc Username không được để trống.")
            .Length(3, 50).WithMessage("Email hoặc Username phải từ 3 đến 50 ký tự.")
            .Matches(@"^(^[a-zA-Z0-9_]{3,50}$|^[^@\s]+@[^@\s]+\.[^@\s]+$)$")
            .WithMessage("Email hoặc Username không hợp lệ.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống.")
            .MinimumLength(8).WithMessage("Password phải có ít nhất 8 ký tự.")
            .Matches(@"[A-Z]").WithMessage("Password phải chứa ít nhất 1 chữ cái viết hoa.")
            .Matches(@"[a-z]").WithMessage("Password phải chứa ít nhất 1 chữ cái viết thường.")
            .Matches(@"[0-9]").WithMessage("Password phải chứa ít nhất 1 chữ số.")
            .Matches(@"[@$!%*?&]").WithMessage("Password phải chứa ít nhất 1 ký tự đặc biệt.");
    }

    protected override bool PreValidate(ValidationContext<LoginDTO> context, ValidationResult result)
    {
        var dto = context.InstanceToValidate;
        if ( dto != null)
        {
            if (!string.IsNullOrWhiteSpace(dto.EmailOrUsername))
            {
                dto.EmailOrUsername = dto.EmailOrUsername.Trim().ToLower();
            }
        }
        return true;
    }
}