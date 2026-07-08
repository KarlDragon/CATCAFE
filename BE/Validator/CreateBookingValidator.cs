using FluentValidation;
using BE.DTOs;
public class CreateBookingValidator : AbstractValidator<CreateBookingDTO>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.TableID)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(0).WithMessage("ID bàn phải lớn hơn 0.");

        RuleFor(x => x.BookedTime)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(DateTime.Now).WithMessage("Thời gian đặt phải lớn hơn thời gian hiện tại.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("Không được để trống.")
            .GreaterThan(x => x.BookedTime).WithMessage("Thời gian kết thúc phải lớn hơn thời gian đặt.");
        
        RuleFor(x => x.BookingCats)
            .Must( list => list.Select(x => x.CatID).Distinct().Count() == list.Count()).WithMessage("Danh sách mèo không được có ID trùng nhau.");
        
        RuleFor(x => x.BookingDetails)
            .Must( list => list.Select(x => x.FoodDrinkID).Distinct().Count() == list.Count()).WithMessage("Danh sách món ăn không được có ID trùng nhau.");
    }
    
}