using FluentValidation;
using BE.DTOs;
public class CreateBookingDetailValidator : AbstractValidator<CreateBookingDetailDTO>
{
    public CreateBookingDetailValidator()
    {
        RuleFor(x => x.FoodDrinkID)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("ID món ăn/đồ uống phải lớn hơn 0.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0.");

        RuleFor(x => x.PriceAtBooking)
            .NotEmpty().WithMessage("Không được để trống.");
    }
}
