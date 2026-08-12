using FluentValidation;
using BE.DTOs;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDTO>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.BookingID)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0.");
    }
}