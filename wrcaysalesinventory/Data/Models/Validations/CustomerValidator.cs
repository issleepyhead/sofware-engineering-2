using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator() {
            RuleFor(x => x.FirstName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Please provide a name.")
                    .MaximumLength(100)
                    .WithMessage("First name must be at least 3 o less than 100 characters. ")
                    .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                    .WithMessage("Too many white spaces.")
                    .MinimumLength(2)
                    .WithMessage("First name must be at least 2 characters");

                RuleFor(x => x.LastName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Please provide a name.")
                    .MinimumLength(2)
                    .WithMessage("Last name must be at least 2 or less than 100 characters.")
                    .MaximumLength(100)
                    .WithMessage("Last name must be at least 3 o less than 100 characters. ")
                    .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                    .WithMessage("Too many white spaces.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
                .WithMessage("Please provide a valid email.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.")
                .Matches(@"^[a-ZA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please provide a phone number.")
                .Matches(@"^(\+639|09)\d{2}[-\s]?\d{3}[-\s]?\d{4}$")
                .WithMessage("Please provide a valid contact information.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");
        }
    }
}
