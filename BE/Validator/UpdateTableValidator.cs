using FluentValidation;
using BE.DTOs;
public class UpdateTableValidator : AbstractValidator<UpdateTableDTO>
{
    public UpdateTableValidator()
    {
        RuleFor(x => x.TableID).GreaterThan(0).WithMessage("Mã bàn không hợp lệ");

        RuleFor(x => x.TableName)
            .MaximumLength(50).WithMessage("Độ dài tên không được quá 50 ký tự")
            .Must( name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tên không được chỉ chứa khoảng trống")
            .When( x => x.TableName != null);
            
        RuleFor(x => x.SeatAmount)
            .GreaterThan(0).WithMessage("Số lượng ghế phải lớn hơn 0!")
            .When( x => x.SeatAmount != null);
    }
    
}