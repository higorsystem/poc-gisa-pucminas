using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using AutoMapper;
using GISA.Authentication.Application.Exceptions;
using GISA.Authentication.Application.Helpers;
using GISA.Authentication.Application.Notifications;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GISA.Authentication.Application.Interfaces;
using GISA.Commons.SDK.AWS;
using Microsoft.IdentityModel.Tokens;

namespace GISA.Authentication.Application.Services
{
    public class AuthenticationService : NotificationHandler, IAuthenticationService
    {
        private readonly IAmazonCognitoIdentityProvider _amazonCognitoIdentityProvider;
        private readonly ICloudConfigurationService _cloudConfigurationService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        private readonly NotificationContext _notificationContext;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserPool _userPool;

        public AuthenticationService
        (
            IMapper mapper,
            ICloudConfigurationService cloudConfigurationService,
            NotificationContext notificationContext,
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool userPool,
            IAmazonCognitoIdentityProvider amazonCognitoIdentityProvider,
            ILogger<AuthenticationService> logger
        )
            : base(cloudConfigurationService, logger, notificationContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cloudConfigurationService = cloudConfigurationService ?? throw new ArgumentNullException(nameof(cloudConfigurationService)); 
            _notificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext)); 
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); 
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager)); 
            _userPool = userPool ?? throw new ArgumentNullException(nameof(userPool)); 
            _amazonCognitoIdentityProvider = amazonCognitoIdentityProvider ?? throw new ArgumentNullException(nameof(amazonCognitoIdentityProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<bool> ChangeEmailAsync(ChangeEmailRequestViewModel changeEmailRequestViewModel)
        {
            try
            {
                var changeEmail = BuildMapper<ChangeEmail>(changeEmailRequestViewModel);

                CheckAndRegisterInvalidNotifications(changeEmail);

                if (changeEmail.Invalid) return false;

                var user = await FindUserByNameAsync(changeEmail.UserName);

                if (user != null)
                    using (var identityProviderClient = IdentityProvider)
                        await identityProviderClient.AdminDeleteUserAsync(BuildAdminDeleteUserRequest(changeEmail));

                await SignUpAsync(BuildMapper<SignUpRequestViewModel>(changeEmailRequestViewModel));

                return true;
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequestViewModel changePasswordRequestViewModel)
        {
            try
            {
                var changePassword = BuildMapper<ChangePassword>(changePasswordRequestViewModel);

                CheckAndRegisterInvalidNotifications(changePassword);

                if (changePassword.Invalid) return false;

                var user = await FindUserByNameAsync(changePassword.UserName);

                if (user != null)
                    using (var identityProviderClient = IdentityProvider)
                        await identityProviderClient.AdminDeleteUserAsync(BuildAdminDeleteUserRequest(changePassword));

                await SignUpAsync(BuildMapper<SignUpRequestViewModel>(changePasswordRequestViewModel));

                return true;
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<bool> ConfirmSignUpAsync(ConfirmSignUpRequestViewModel confirmSignUpViewModel)
        {
            try
            {
                var confirmSignUp = BuildMapper<ConfirmSignUp>(confirmSignUpViewModel);

                CheckAndRegisterInvalidNotifications(confirmSignUp);

                if (confirmSignUp.Invalid) return false;

                var user = await FindUserByNameAsync(confirmSignUp.UserName);

                ValidateUserNotFoundException(user, confirmSignUp.UserName);

                var confirmedUser = await _amazonCognitoIdentityProvider.ConfirmSignUpAsync(BuildConfirmSignUpRequest(confirmSignUp)).ConfigureAwait(false);

                //TODO: Estava como true.
                return confirmedUser.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (UserNotFoundException userNotFoundException)
            {
                AddUserNotFoundExceptionNotification(userNotFoundException);
            }
            catch (UserNotConfirmedException userNotConfirmedException)
            {
                AddUserNotConfirmedExceptionNotification(userNotConfirmedException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestViewModel forgotPasswordViewModel)
        {
            try
            {
                var forgotPassword = BuildMapper<ForgotPassword>(forgotPasswordViewModel);

                CheckAndRegisterInvalidNotifications(forgotPassword);

                if (forgotPassword.Invalid) return false;

                var user = await FindUserByNameAsync(forgotPassword.UserName);

                ValidateUserNotFoundException(user, forgotPassword.UserName);

                using (var identityProviderClient = IdentityProvider)
                    await identityProviderClient.ForgotPasswordAsync(BuildForgotPasswordRequest(forgotPassword.UserName));

                return true;
            }
            catch (UserNotFoundException userNotFoundException)
            {
                AddUserNotFoundExceptionNotification(userNotFoundException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginRequestViewModel loginViewModel)
        {
            try
            {
                var login = BuildMapper<Login>(loginViewModel);

                CheckAndRegisterInvalidNotifications(login);

                if (login.Invalid) return null;

                using (var identityProviderClient = IdentityProvider)
                {
                    AddLoginParameters(loginViewModel);
                    InitiateAuthResponse authResponse = await identityProviderClient.InitiateAuthAsync(LoginIdentityProviderRequest);
                    return _mapper.Map<LoginResponseViewModel>(authResponse.AuthenticationResult);
                }
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                AddNotAuthorizedExceptionNotification(notAuthorizedException);
            }
            catch (UserNotConfirmedException userNotConfirmedException)
            {
                AddUserNotConfirmedExceptionNotification(userNotConfirmedException);
            }
            catch (AmazonServiceException amazonServiceException)
            {
                AddAmazonServiceExceptionNotification(amazonServiceException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return new LoginResponseViewModel();
        } 
        
        public async Task<LoginResponseViewModel> LoginJwtTokenChangedAsync(LoginRequestViewModel loginViewModel)
        {
            try
            {
                var login = BuildMapper<Login>(loginViewModel);

                CheckAndRegisterInvalidNotifications(login);

                if (login.Invalid) return null;

                using (var identityProviderClient = IdentityProvider)
                {
                    AddLoginParameters(loginViewModel);

                    var result = await _signInManager.PasswordSignInAsync
                    (
                        loginViewModel.UserName, 
                        loginViewModel.Password,
                        loginViewModel.RememberMe,
                        lockoutOnFailure: false
                    );

                    if (result.Succeeded)
                    {
                        var findUser = await _userManager.FindByNameAsync(loginViewModel.UserName);
                        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(findUser);

                        InitiateAuthResponse authResponse = await identityProviderClient.InitiateAuthAsync(LoginIdentityProviderRequest);
                        var response = _mapper.Map<LoginResponseViewModel>(authResponse.AuthenticationResult);
                        response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                        return response;
                    }

                    return null;
                }
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                AddNotAuthorizedExceptionNotification(notAuthorizedException);
            }
            catch (UserNotConfirmedException userNotConfirmedException)
            {
                AddUserNotConfirmedExceptionNotification(userNotConfirmedException);
            }
            catch (AmazonServiceException amazonServiceException)
            {
                AddAmazonServiceExceptionNotification(amazonServiceException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return new LoginResponseViewModel();
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(CognitoUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Attributes.FirstOrDefault(a => a.Key == "email").Value),
                    new Claim(ClaimTypes.NameIdentifier, user.Attributes.FirstOrDefault(a => a.Key == "sub").Value),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString())
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cloudConfigurationService.GetKey()));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _cloudConfigurationService.GetIssuer(),
                audience: _cloudConfigurationService.GetAudience(),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_cloudConfigurationService.GetDurationInMinutes()),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<LoginResponseViewModel> RefreshTokenAsync(RefreshTokenRequestViewModel refreshTokenViewModel)
        {
            try
            {
                var refreshToken = BuildMapper<RefreshToken>(refreshTokenViewModel);

                CheckAndRegisterInvalidNotifications(refreshToken);

                if (refreshToken.Invalid) return null;

                using (var identityProviderClient = IdentityProvider)
                {
                    AddRefreshTokenParameters(refreshTokenViewModel);
                    InitiateAuthResponse authResponse = await identityProviderClient.InitiateAuthAsync(RefreshTokenIdentityProviderRequest);
                    return _mapper.Map<LoginResponseViewModel>(authResponse.AuthenticationResult);
                }
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                AddNotAuthorizedExceptionNotification(notAuthorizedException);
            }
            catch (UserNotConfirmedException userNotConfirmedException)
            {
                AddUserNotConfirmedExceptionNotification(userNotConfirmedException);
            }
            catch (AmazonServiceException amazonServiceException)
            {
                AddAmazonServiceExceptionNotification(amazonServiceException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return new LoginResponseViewModel();
        }

        public async Task<bool> ResendConfirmationCodeAsync(ResendConfirmationCodeRequestViewModel resendConfirmationCodeViewModel)
        {
            try
            {
                var resendConfirmationCode = BuildMapper<ResendConfirmationCode>(resendConfirmationCodeViewModel);

                CheckAndRegisterInvalidNotifications(resendConfirmationCode);

                if (resendConfirmationCode.Invalid) return false;

                var user = await FindUserByNameAsync(resendConfirmationCode.UserName);

                ValidateUserNotFoundException(user, resendConfirmationCode.UserName);
                ValidateUserAlreadyConfirmedException(user, resendConfirmationCode.UserName);

                using (var identityProviderClient = IdentityProvider)
                    await identityProviderClient.ResendConfirmationCodeAsync(
                        BuildResendConfirmationCodeRequest(resendConfirmationCodeViewModel.UserName));

                return true;
            }
            catch (UserNotFoundException userNotFoundException)
            {
                AddUserNotFoundExceptionNotification(userNotFoundException);
            }
            catch (UserAlreadyConfirmedException userAlreadyConfirmedException)
            {
                AddUserAlreadyConfirmedExceptionNotification(userAlreadyConfirmedException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestViewModel resetPasswordRequestViewModel)
        {
            try
            {
                var resetPassword = BuildMapper<ResetPassword>(resetPasswordRequestViewModel);

                CheckAndRegisterInvalidNotifications(resetPassword);

                if (resetPassword.Invalid) return false;

                var user = await FindUserByNameAsync(resetPassword.UserName);

                ValidateUserNotFoundException(user, resetPassword.UserName);

                using (var identityProviderClient = IdentityProvider)
                    await identityProviderClient.ConfirmForgotPasswordAsync(BuildConfirmForgotPasswordRequest(resetPassword));

                return true;
            }
            catch (UserNotFoundException userNotFoundException)
            {
                AddUserNotFoundExceptionNotification(userNotFoundException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<bool> SignOutAsync(SignOutRequestViewModel signOutViewModel)
        {
            try
            {
                var signOut = BuildMapper<SignOut>(signOutViewModel);

                CheckAndRegisterInvalidNotifications(signOut);

                if (signOut.Invalid) return false;

                var user = await FindUserByNameAsync(signOut.UserName);

                ValidateUserNotFoundException(user, signOut.UserName);

                user.SignOut();

                return true;
            }
            catch (UserNotFoundException userNotFoundException)
            {
                AddUserNotFoundExceptionNotification(userNotFoundException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return false;
        }

        public async Task<SignUpResponseViewModel> SignUpAsync(SignUpRequestViewModel signUpViewModel)
        {
            try
            {
                var signUp = BuildMapper<SignUp>(signUpViewModel);

                CheckAndRegisterInvalidNotifications(signUp);

                if (signUp.Invalid) return null;

                var user = await FindUserByNameAsync(signUp.UserName) ?? BuildUserObject(signUp.UserName);

                if (user?.Status != null) throw new UsernameExistsException($"The user {signUp.UserName} already exists");

                await _amazonCognitoIdentityProvider.SignUpAsync(BuildSignUpRequest(signUp)).ConfigureAwait(false);

                return _mapper.Map<SignUpResponseViewModel>(signUp);
            }
            catch (UsernameExistsException usernameExistsException)
            {
                AddUsernameExistsExceptionNotification(usernameExistsException);
            }
            catch (Exception exception)
            {
                AddGenericExceptionValidatorNotification(exception);
            }

            return new SignUpResponseViewModel();
        }

        private void AddAmazonServiceExceptionNotification(AmazonServiceException amazonServiceException)
        {
            _notificationContext.AddNotification("ServiceExceptionValidator", amazonServiceException.Message);
            _logger.LogError($"ServiceExceptionValidator: {amazonServiceException.Message}");
        }

        private void AddGenericExceptionValidatorNotification(Exception genericException)
        {
            _notificationContext.AddNotification("GenericExceptionValidator", genericException.Message);
            _logger.LogError($"GenericExceptionValidator: {genericException.Message}");
        }

        private void AddLoginParameters(LoginRequestViewModel loginViewModel)
        {
            LoginIdentityProviderRequest.AuthParameters["USERNAME"] = loginViewModel.UserName;
            LoginIdentityProviderRequest.AuthParameters["PASSWORD"] = loginViewModel.Password;
            LoginIdentityProviderRequest.AuthParameters["SECRET_HASH"] = BuildSecretHash(loginViewModel.UserName);
        }

        private void AddNotAuthorizedExceptionNotification(NotAuthorizedException notAuthorizedException)
        {
            _notificationContext.AddNotification("NotAuthorizedValidator", notAuthorizedException.Message);
            _logger.LogInformation($"NotAuthorizedValidator: {notAuthorizedException.Message}");
        }

        private void AddRefreshTokenParameters(RefreshTokenRequestViewModel refreshTokenViewModel)
        {
            RefreshTokenIdentityProviderRequest.AuthParameters["USERNAME"] = refreshTokenViewModel.UserName;
            RefreshTokenIdentityProviderRequest.AuthParameters["REFRESH_TOKEN"] = refreshTokenViewModel.Token;
            RefreshTokenIdentityProviderRequest.AuthParameters["SECRET_HASH"] = BuildSecretHash(refreshTokenViewModel.UserName);
        }

        private void AddSignUpParameters(SignUpRequest signUpRequest, SignUp signUp)
        {
            signUpRequest.UserAttributes.Add(new AttributeType() { Name = "name", Value = signUp.UserName });
            signUpRequest.UserAttributes.Add(new AttributeType() { Name = "email", Value = signUp.Email });
        }



        private AdminDeleteUserRequest BuildAdminDeleteUserRequest(ChangePassword changePassword)
        {
            return new AdminDeleteUserRequest() { Username = changePassword.UserName, UserPoolId = _userPool.PoolID };
        }

        private AdminDeleteUserRequest BuildAdminDeleteUserRequest(ChangeEmail changeEmail)
        {
            return new AdminDeleteUserRequest() { Username = changeEmail.UserName, UserPoolId = _userPool.PoolID };
        }

        private ConfirmForgotPasswordRequest BuildConfirmForgotPasswordRequest(ResetPassword resetPassword)
        {
            return new ConfirmForgotPasswordRequest()
            {
                ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                Username = resetPassword.UserName,
                ConfirmationCode = resetPassword.Code,
                Password = resetPassword.Password,
                SecretHash = BuildSecretHash(resetPassword.UserName)
            };
        }

        private ConfirmSignUpRequest BuildConfirmSignUpRequest(ConfirmSignUp confirmSignUp)
        {
            var confirmSignUpRequest = new ConfirmSignUpRequest
            {
                ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                SecretHash = BuildSecretHash(confirmSignUp.UserName),
                Username = confirmSignUp.UserName,
                ConfirmationCode = confirmSignUp.Code
            };

            return confirmSignUpRequest;
        }

        private ForgotPasswordRequest BuildForgotPasswordRequest(string userName)
        {
            return new ForgotPasswordRequest()
            {
                ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                Username = userName,
                SecretHash = BuildSecretHash(userName)
            };
        }

        private TModel BuildMapper<TModel>(object source)
        {
            return _mapper.Map<TModel>
                               (source, mapperOptions => mapperOptions
                                .AfterMap((_, instance) =>
                                {
                                    if (instance is EntityBase) ((EntityBase)instance).Validate(instance);
                                }));
        }

        private ResendConfirmationCodeRequest BuildResendConfirmationCodeRequest(string userName)
        {
            return new ResendConfirmationCodeRequest()
            {
                ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                Username = userName,
                SecretHash = BuildSecretHash(userName)
            };
        }

        private string BuildSecretHash(string userName)
        {
            return CognitoHashCalculatorHelper.GetUserPoolSecretHash(userName,
                                _cloudConfigurationService.GetUserPoolClientId(),
                                _cloudConfigurationService.GetUserPoolClientSecret());
        }

        private SignUpRequest BuildSignUpRequest(SignUp signUp)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                SecretHash = BuildSecretHash(signUp.UserName),
                Username = signUp.UserName,
                Password = signUp.Password,
            };

            AddSignUpParameters(signUpRequest, signUp);

            return signUpRequest;
        }

        private CognitoUser BuildUserObject(string userName)
        {
            return new CognitoUser(userName, _cloudConfigurationService.GetUserPoolClientId(), _userPool, IdentityProvider);
        }

        private async Task<CognitoUser> FindUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
        }

        private void ValidateUserAlreadyConfirmedException(CognitoUser user, string userName)
        {
            if (!"UNCONFIRMED".Equals(user.Status)) throw new UserAlreadyConfirmedException($"The user {userName} is already confirmed");
        }

        private void ValidateUserNotFoundException(CognitoUser user, string userName)
        {
            if (user == null) throw new UserNotFoundException($"The user {userName} was not found");
        }
    }
}