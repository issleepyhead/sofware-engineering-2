using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(productModel => productModel.ProductName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Product name cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Product name must be at least less than 100 characters.")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters long.");

            RuleFor(productModel => productModel.ProductPrice)
                .Cascade(CascadeMode.Stop)
                .Matches(@"^^(?!(\d+)?\.?(\d+)$).*$")
                .WithMessage("Please enter a valid product price.");

            RuleFor(productModel => productModel.ProductCost)
                .Cascade(CascadeMode.Stop)
                .Matches(@"^(?!(\d+)?\.?(\d+)$).*$")
                .WithMessage("Please enter a valid product cost.");

            RuleFor(productModel => productModel.ProductUnit)
                .Cascade(CascadeMode.Stop);
            RuleFor(productModel => productModel.ProductDescription)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300)
                .WithMessage("Product description must be at least less than 300 characters");

        }
    }
}
