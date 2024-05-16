using FluentValidation;
using System;
using System.Text.RegularExpressions;
using System.Windows.Xps;

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
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.")
                .MaximumLength(100)
                .WithMessage("Product name must be at least less than 100 characters.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.");

            RuleFor(productModel => productModel.ProductPrice)
                .NotEmpty()
                .WithMessage("Please provide a selling price.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.")
                .Matches(@"^(\d+)?\.?(\d+)$")
                .WithMessage("Please enter a valid product price.");

            RuleFor(productModel => productModel.ProductCost)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please provide a cost post.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.")
                .Matches(@"^(\d+)?\.?(\d+)$")
                .WithMessage("Please enter a valid product cost.");

            RuleFor(productModel => productModel.ProductUnit)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithErrorCode("Please provide a product unit")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(productModel => productModel.ProductDescription)
                .Cascade(CascadeMode.Stop)
                
                .MaximumLength(300)
                .WithMessage("Product description must be at least less than 300 characters")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

        }
    }
}
