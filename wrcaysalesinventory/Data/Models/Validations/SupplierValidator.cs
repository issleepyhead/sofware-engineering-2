using FluentValidation;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class SupplierValidator : AbstractValidator<SupplierModel>
    {
        public SupplierValidator()
        {
            RuleFor(sModel => sModel.SupplierName)
                .Cascade(CascadeMode.Stop)
                .Empty()
                .WithMessage("Supplier Name can't be empty.");

            RuleFor(sModel => sModel.City)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("City must be at least less than 100 characters.");

            RuleFor(sModel => sModel.Country)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("Country must be at least less than 100 characters."); ;

            RuleFor(sModel => sModel.Address)
                .Cascade(CascadeMode.Stop)
                .Empty()
                .WithMessage("Address can't be empty.")
                .MaximumLength(300)
                .WithMessage("Address must be at least less than 300 characters."); ;

            RuleFor(sModel => sModel.PhoneNumbebr)
                .Cascade(CascadeMode.Stop)
                .Empty()
                .WithMessage("Phone Number can't be empty.")
                .MaximumLength(50)
                .WithMessage("Phone Number must be at least less than 50 characters.");

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("First Name must be at least less than 100 characters."); ;

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100)
                .WithMessage("Last Name must be at least less than 100 characters."); ;

        }
    }
}
