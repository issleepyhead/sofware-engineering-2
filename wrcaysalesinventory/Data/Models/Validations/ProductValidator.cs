using FluentValidation;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(productModel => productModel.ProductName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please provide a product name.")
                .MaximumLength(100)
                .WithMessage("Product name must be at least less than 100 characters.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.");

            RuleFor(productModel => productModel.ProductPrice)
                .NotEmpty()
                .WithMessage("Please provide a selling price.")
                .Matches(@"^(\d+)?\.?(\d+)$")
                .WithMessage("Please enter a valid product price.");

            RuleFor(productModel => productModel.ProductCost)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please provide a cost price.")
                .Matches(@"^(\d+)?\.?(\d+)$")
                .WithMessage("Please enter a valid product cost.");

            RuleFor(productModel => productModel.ProductUnit)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithErrorCode("Please provide a product unit");

            RuleFor(productModel => productModel.ProductDescription)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300)
                .WithMessage("Product description must be at least less than 300 characters");

        }
    }
}
