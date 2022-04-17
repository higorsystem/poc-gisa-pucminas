using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class ResetPassword : EntityBase
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<ResetPassword>()
        {
            return new ResetPasswordValidator().Validate(this);
        }
    }
}