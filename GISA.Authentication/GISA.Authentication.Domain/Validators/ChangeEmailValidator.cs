using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmail>
    {
        public ChangeEmailValidator()
        {
            RuleFor(changeEmailValidator => changeEmailValidator.UserName)
                .NotEmpty();

            RuleFor(changeEmailValidator => changeEmailValidator.Password)
                .MinimumLength(6)
                .NotEmpty();

            RuleFor(changeEmailValidator => changeEmailValidator.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(changeEmailValidator => changeEmailValidator.ConfirmPassword)
                .Equal(changeEmailValidator => changeEmailValidator.Password)
                .WithMessage("Passwords do not match");
        }
    }
}