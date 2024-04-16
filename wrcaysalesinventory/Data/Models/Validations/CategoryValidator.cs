using FluentValidation;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(categoryModel => categoryModel.CategoryName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please enter a valid category name.")
                .MinimumLength(3)
                .WithMessage("Category name must be at least 3 or less than 50 characters.");
                
            RuleFor(categoryModel => categoryModel.CategoryDescription)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300)
                .WithMessage("Description must be at least 300 characters or less.");
        }
    }
}
