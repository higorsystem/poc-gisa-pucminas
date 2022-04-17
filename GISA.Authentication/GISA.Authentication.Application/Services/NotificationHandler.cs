using System;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using GISA.Authentication.Application.Exceptions;
using GISA.Authentication.Application.Notifications;
using GISA.Authentication.Domain.Entities;
using GISA.Commons.SDK.AWS;
using Microsoft.Extensions.Logging;

namespace GISA.Authentication.Application.Services
{
    public abstract class NotificationHandler
    {
        private readonly ICloudConfigurationService _cloudConfigurationService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly NotificationContext _notificationContext;
        private InitiateAuthRequest _loginIdentityProviderRequest;
        private InitiateAuthRequest _refreshTokenIdentityProviderRequest;
        private RegionEndpoint _regionEndpoint;

        protected NotificationHandler(ICloudConfigurationService cloudConfigurationService, ILogger<AuthenticationService> logger, NotificationContext notificationContext)
        {
            _cloudConfigurationService = cloudConfigurationService ?? throw new ArgumentNullException(nameof(cloudConfigurationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
        }

        protected AmazonCognitoIdentityProviderClient IdentityProvider => new AmazonCognitoIdentityProviderClient(RegionEndpoint);

        protected InitiateAuthRequest LoginIdentityProviderRequest
        {
            get
            {
                if (_loginIdentityProviderRequest == null)
                    _loginIdentityProviderRequest = new InitiateAuthRequest
                    {
                        ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                        AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                        AuthParameters = new Dictionary<string, string>(){
                            {"USERNAME",string.Empty},
                            {"PASSWORD",string.Empty},
                            {"SECRET_HASH", string.Empty}
                        }
                    };

                return _loginIdentityProviderRequest;
            }
        }

        protected InitiateAuthRequest RefreshTokenIdentityProviderRequest
        {
            get
            {
                if (_refreshTokenIdentityProviderRequest == null)
                    _refreshTokenIdentityProviderRequest = new InitiateAuthRequest
                    {
                        ClientId = _cloudConfigurationService.GetUserPoolClientId(),
                        AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                        AuthParameters = new Dictionary<string, string>(){
                            {"USERNAME",string.Empty},
                            {"SECRET_HASH", string.Empty}
                        }
                    };

                return _refreshTokenIdentityProviderRequest;
            }
        }

        protected void AddUserAlreadyConfirmedExceptionNotification(UserAlreadyConfirmedException userAlreadyConfirmedException)
        {
            _notificationContext.AddNotification("UserAlreadyConfirmedExceptionValidator", userAlreadyConfirmedException.Message);
            _logger.LogInformation($"UserAlreadyConfirmedExceptionValidator: {userAlreadyConfirmedException.Message}");
        }

        protected void AddUsernameExistsExceptionNotification(UsernameExistsException usernameExistsException)
        {
            _notificationContext.AddNotification("UsernameExistsExceptionValidator", usernameExistsException.Message);
            _logger.LogInformation($"UsernameExistsExceptionValidator: {usernameExistsException.Message}");
        }

        protected void AddUserNotConfirmedExceptionNotification(UserNotConfirmedException userNotConfirmedException)
        {
            _notificationContext.AddNotification("UserNotConfirmedExceptionValidator", userNotConfirmedException.Message);
            _logger.LogInformation($"UserNotConfirmedExceptionValidator: {userNotConfirmedException.Message}");
        }

        protected void AddUserNotFoundExceptionNotification(UserNotFoundException userNotFoundException)
        {
            _notificationContext.AddNotification("UserNotFoundExceptionValidator", userNotFoundException.Message);
            _logger.LogInformation($"UserNotFoundExceptionValidator: {userNotFoundException.Message}");
        }

        protected void CheckAndRegisterInvalidNotifications(EntityBase entityBase)
        {
            if (entityBase.Invalid) _notificationContext.AddNotifications(entityBase.ValidationResult);
        }

        private RegionEndpoint RegionEndpoint
        {
            get
            {
                return _regionEndpoint ??= RegionEndpoint.GetBySystemName(_cloudConfigurationService.GetRegion());
            }
        }

    }
}