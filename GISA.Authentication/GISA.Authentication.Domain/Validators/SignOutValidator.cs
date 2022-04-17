using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class SignOutValidator : AbstractValidator<SignOut>
    {
        public SignOutValidator()
        {
            RuleFor(signOutValidator => signOutValidator.UserName)
                .NotEmpty();
        }
    }
}