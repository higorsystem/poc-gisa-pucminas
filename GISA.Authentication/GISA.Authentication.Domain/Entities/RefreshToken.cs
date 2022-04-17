using FluentValidation.Results;
using GISA.Authentication.Domain.Validators;

namespace GISA.Authentication.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public string Token { get; set; }
        public string UserName { get; set; }

        public override ValidationResult GetValidationResult<RefreshToken>()
        {
            return new RefreshTokenValidator().Validate(this);
        }
    }
}