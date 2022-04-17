using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class SignUpValidator : AbstractValidator<SignUp>
    {
        public SignUpValidator()
        {
            RuleFor(signUpValidator => signUpValidator.UserName)
                .NotEmpty();

            RuleFor(signUpValidator => signUpValidator.Password)
                .MinimumLength(6)
                .NotEmpty();

            RuleFor(signUpValidator => signUpValidator.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(signUpValidator => signUpValidator.ConfirmPassword)
                .Equal(signUpValidator => signUpValidator.Password)
                .WithMessage("Passwords do not match");
        }
    }
}