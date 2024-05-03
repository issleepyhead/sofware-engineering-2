using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator() {
            RuleFor(x=> x.FullName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Please provide a name.")
                    .MinimumLength(3)
                    .WithMessage("Category name must be at least 3 or less than 100 characters.")
                    .MaximumLength(100)
                    .WithMessage("Category name must be at least 3 o less than 100 characters. ");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
                .WithMessage("Please provide a valid email.");

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please provide a phone number.")
                .Matches(@"^(\+639|09)\d{2}[-\s]?\d{3}[-\s]?\d{4}$")
                .WithMessage("Please provide a valid contact information.");
        }
    }
}
