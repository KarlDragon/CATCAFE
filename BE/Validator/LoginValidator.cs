using BE.DTOs;
using FluentValidation;

public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email hoặc Username không được để trống.")
            .Length(3, 50).WithMessage("Email hoặc Username phải từ 3 đến 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Email hoặc Username chỉ được chứa chữ cái, chữ số và dấu gạch dưới (_).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống.")
            .MinimumLength(8).WithMessage("Password phải có ít nhất 8 ký tự.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt.");
    }
}