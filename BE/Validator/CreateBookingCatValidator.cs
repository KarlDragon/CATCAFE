using FluentValidation;
using BE.DTOs;
public class CreateBookingCatValidator : AbstractValidator<CreateBookingCatDTO>
{
    public CreateBookingCatValidator()
    {
        RuleFor(x => x.CatID)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("ID mèo phải lớn hơn 0.");
    }
}
