using FluentValidation;
using BE.Models;
public class CreateTableValidator : AbstractValidator<Table>
{
    public CreateTableValidator()
    {
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("Không được để trống.")
            .MaximumLength(50).WithMessage("Độ dài tên không được quá 50 ký tự");

        RuleFor(x => x.SeatAmount)
            .GreaterThan(0).WithMessage("Số lượng ghế phải lớn hơn 0!");
    }
    
}