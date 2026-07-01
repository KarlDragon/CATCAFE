using FluentValidation;
using BE.DTOs;
public class UpdateCatValidator : AbstractValidator<UpdateCatDTO>
{
    public UpdateCatValidator()
    {
        RuleFor(x => x.CatID).GreaterThan(0).WithMessage("Mã bàn không hợp lệ");

        RuleFor(x => x.CatName)
            .MaximumLength(50).WithMessage("Độ dài tên không được quá 50 ký tự")
            .Must( name => !string.IsNullOrWhiteSpace(name)).WithMessage("Tên không được chỉ chứa khoảng trống")
            .When( x => x.CatName != null);
            
        RuleFor(x => x.Breed)
            .MaximumLength(50).WithMessage("Độ dài giống mèo không được quá 50 ký tự")
            .Must( breed => !string.IsNullOrWhiteSpace(breed)).WithMessage("Tên không được chỉ chứa khoảng trống")
            .When( x => x.Breed != null);

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Độ dài trạng thái không được quá 50 ký tự")
            .Must( status => !string.IsNullOrWhiteSpace(status)).WithMessage("Tên không được chỉ chứa khoảng trống")
            .When( x => x.Status != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Độ dài mô tả không được quá 500 ký tự")
            .Must( description => !string.IsNullOrWhiteSpace(description)).WithMessage("Mô tả không được chỉ chứa khoảng trống")
            .When( x => x.Description != null);
    }
    
}