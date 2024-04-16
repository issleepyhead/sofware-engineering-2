using FluentValidation;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class SupplierValidator : AbstractValidator<SupplierModel>
    {
        public SupplierValidator()
        {
            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.LastName)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.SupplierName)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.City)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.Country)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.Address)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.PhoneNumbebr)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop);

            RuleFor(sModel => sModel.FirstName)
                .Cascade(CascadeMode.Stop);

        }
    }
}
