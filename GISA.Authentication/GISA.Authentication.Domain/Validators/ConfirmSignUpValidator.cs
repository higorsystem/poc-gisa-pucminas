using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class ConfirmSignUpValidator : AbstractValidator<ConfirmSignUp>
    {
        public ConfirmSignUpValidator()
        {
            RuleFor(confirmSignUpValidator => confirmSignUpValidator.UserName)
                .NotEmpty();

            RuleFor(confirmSignUpValidator => confirmSignUpValidator.Code)
                .NotEmpty();
        }
    }
}