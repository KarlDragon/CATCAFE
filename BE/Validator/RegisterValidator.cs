using BE.DTOs;
using FluentValidation;
using FluentValidation.Results;

public class RegisterValidator : AbstractValidator<RegisterDTO>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống.")
            .Length(3, 50).WithMessage("Username phải từ 3 đến 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username chỉ được chứa chữ cái, chữ số và dấu gạch dưới (_).");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không hợp lệ.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống.")
            .MinimumLength(8).WithMessage("Password phải có ít nhất 8 ký tự.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password phải chứa ít nhất một chữ cái viết thường, một chữ cái viết hoa, một chữ số và một ký tự đặc biệt.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name không được để trống.")
            .Length(3, 100).WithMessage("Name phải từ 3 đến 100 ký tự.");
    }

    protected override bool PreValidate(ValidationContext<RegisterDTO> context, ValidationResult result)
    {
        var dto = context.InstanceToValidate;
        if ( dto != null)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                dto.Username = dto.Username.Trim().ToLower();
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                dto.Email = dto.Email.Trim().ToLower();
            }
        }
        return true;
    }
}