using FluentValidation;
using BE.DTOs;
public class CreateCatValidator : AbstractValidator<CreateCatDTO>
{
    public CreateCatValidator()
    {
        RuleFor(x => x.CatName)
            .NotEmpty().WithMessage("Không được để trống.")
            .MaximumLength(50).WithMessage("Độ dài tên không được quá 50 ký tự");

        RuleFor(x => x.Breed)
            .NotEmpty().WithMessage("Không được để trống.")
            .MaximumLength(50).WithMessage("Độ dài giống mèo không được quá 50 ký tự");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Không được để trống.")
            .MaximumLength(50).WithMessage("Độ dài trạng thái không được quá 50 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Độ dài mô tả không được quá 500 ký tự");
    }
    
}