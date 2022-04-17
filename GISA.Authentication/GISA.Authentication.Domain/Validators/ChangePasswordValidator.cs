using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(changePasswordValidator => changePasswordValidator.UserName)
                .NotEmpty();

            RuleFor(changePasswordValidator => changePasswordValidator.Password)
                .MinimumLength(6)
                .NotEmpty();

            RuleFor(changePasswordValidator => changePasswordValidator.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(changePasswordValidator => changePasswordValidator.ConfirmPassword)
                .Equal(changePasswordValidator => changePasswordValidator.Password)
                .WithMessage("Passwords do not match");
        }
    }
}