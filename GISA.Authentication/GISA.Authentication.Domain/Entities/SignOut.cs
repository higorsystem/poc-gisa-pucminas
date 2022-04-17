using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class SignOut : EntityBase
    {
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<SignOut>()
        {
            return new SignOutValidator().Validate(this);
        }
    }
}