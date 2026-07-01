using FluentValidation;
using BE.DTOs;
public class CreateFoodDrinkValidator : AbstractValidator<CreateFoodDrinkDTO>
{
    public CreateFoodDrinkValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Không được để trống.")
            .MaximumLength(50).WithMessage("Độ dài tên không được quá 50 ký tự");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("Giá phải là số dương");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThanOrEqualTo(0).WithMessage("Số lượng không được là số âm");
    }
    
}