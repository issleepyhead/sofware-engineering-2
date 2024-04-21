﻿using FluentValidation;

namespace wrcaysalesinventory.Data.Models.Validations
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(userModel => userModel.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .MaximumLength(100)
                .WithMessage("First name must be at least 100 characters.");

            RuleFor(userModel => userModel.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .MaximumLength(100)
                .WithMessage("Last name must be at least less than 100 characters.");

            RuleFor(userModel => userModel.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .MaximumLength(300)
                .WithMessage("Address must be at least less than 300 characters.");

            RuleFor(userModel => userModel.Contact)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .Matches(@"^(\+639|09)\d{2}[-\s]?\d{3}[-\s]?\d{4}$")
                .WithMessage("Please provide a valid contact information.");

            RuleFor(userModel => userModel.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .MinimumLength(6)
                .WithMessage("Username must be at least 6 or more characters.");

            RuleFor(userModel => userModel.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("This field can't be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 or more characters.");
        }
    }
}
