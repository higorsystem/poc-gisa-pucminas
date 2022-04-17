using System;
using System.Threading.Tasks;
using GISA.Authentication.Application.Interfaces;
using GISA.Authentication.Application.Notifications;
using GISA.Authentication.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GISA.Authentication.WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService, NotificationContext notificationContext) : base(notificationContext)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmailAsync(ChangeEmailRequestViewModel changeEmailRequestViewModel)
        {
            await _authenticationService.ChangeEmailAsync(changeEmailRequestViewModel);
            return CustomResponse();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequestViewModel changePasswordRequestViewModel)
        {
            await _authenticationService.ChangePasswordAsync(changePasswordRequestViewModel);
            return CustomResponse();
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmSignUpAsync(ConfirmSignUpRequestViewModel confirmSignUpViewModel)
        {
            await _authenticationService.ConfirmSignUpAsync(confirmSignUpViewModel);
            return CustomResponse();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequestViewModel forgotPasswordViewModel)
        {
            await _authenticationService.ForgotPasswordAsync(forgotPasswordViewModel);
            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequestViewModel loginViewModel)
        {
            var loginResponse = await _authenticationService.LoginJwtTokenChangedAsync(loginViewModel);
            return CustomResponse(Ok(loginResponse));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestViewModel refreshTokenViewModel)
        {
            var refreshTokenResponse = await _authenticationService.RefreshTokenAsync(refreshTokenViewModel);
            return CustomResponse(Ok(refreshTokenResponse));
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendConfirmationCodeSignUpAsync(ResendConfirmationCodeRequestViewModel resendConfirmationCodeViewModel)
        {
            await _authenticationService.ResendConfirmationCodeAsync(resendConfirmationCodeViewModel);
            return CustomResponse();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequestViewModel resetPasswordViewModel)
        {
            await _authenticationService.ResetPasswordAsync(resetPasswordViewModel);
            return CustomResponse();
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync(SignOutRequestViewModel signOutViewModel)
        {
            await _authenticationService.SignOutAsync(signOutViewModel);
            return CustomResponse();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(SignUpRequestViewModel signUpViewModel)
        {
            var signUpResponse = await _authenticationService.SignUpAsync(signUpViewModel);
            return CustomResponse(CreatedAtAction("SignUp", signUpResponse));
        }
    }
}