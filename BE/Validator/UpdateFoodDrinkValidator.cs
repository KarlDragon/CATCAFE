using FluentValidation;
using BE.DTOs;
public class UpdateFoodDrinkValidator : AbstractValidator<UpdateFoodDrinkDTO>
{
    public UpdateFoodDrinkValidator()
    {
        RuleFor(x => x.FoodDrinkID).GreaterThan(0).WithMessage("Mã món ăn/đồ uống không hợp lệ");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Độ dài tên không được quá 100 ký tự")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tên không được chỉ chứa khoảng trống")
            .When(x => x.Name != null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá phải lớn hơn 0!")
            .When(x => x.Price != null);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Số lượng phải lớn hơn hoặc bằng 0!")
            .When(x => x.Quantity != null);
    }
    
}