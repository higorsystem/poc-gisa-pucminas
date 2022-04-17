using FluentValidation;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Domain.Validators
{
    public class ResendConfirmationCodeValidator : AbstractValidator<ResendConfirmationCode>
    {
        public ResendConfirmationCodeValidator()
        {
            RuleFor(confirmSignUpValidator => confirmSignUpValidator.UserName)
                .NotEmpty();
        }
    }
}