using FluentValidation;
using System.Text.RegularExpressions;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class SupplierValidator : AbstractValidator<SupplierModel>
    {
        public SupplierValidator()
        {
            RuleFor(sModel => sModel.SupplierName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Supplier name can't be empty.")
                .MaximumLength(100)
                .WithMessage("Supplier name must be at least less than 100 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(sModel => sModel.City)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("City must be at least less than 100 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(sModel => sModel.Country)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("Country must be at least less than 100 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(sModel => sModel.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Address can't be empty.")
                .MaximumLength(300)
                .WithMessage("Address must be at least less than 300 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(sModel => sModel.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Contact can't be empty.")
                .MaximumLength(50)
                .WithMessage("Phone Number must be at least less than 50 characters.")
                .Matches(@"^(\+639|09)\d{2}[-\s]?\d{3}[-\s]?\d{4}$")
                .WithMessage("Please provide a valid contact information.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces."); ;

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("First name must be at least less than 100 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("Last name must be at least less than 100 characters.")
                .Must(x => !Regex.IsMatch(string.IsNullOrEmpty(x) ? "" : x, @"\s{2,}"))
                .WithMessage("Too many white spaces.");

        }
    }
}
