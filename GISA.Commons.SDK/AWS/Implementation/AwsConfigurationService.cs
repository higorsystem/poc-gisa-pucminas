using System;
using Microsoft.Extensions.Configuration;

namespace GISA.Commons.SDK.AWS.Implementation
{
    public class AwsConfigurationService : ICloudConfigurationService
    {
        private readonly IConfiguration _configuration;

        public AwsConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAccessKey()
        {
            return Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ??  _configuration["AWS:AccessKey"];
        } 
        
        public string GetKey()
        {
            return Environment.GetEnvironmentVariable("AWS_JWT_KEY") ??  _configuration["AWS:Key"];
        }
        
        public string GetIssuer()
        {
            return Environment.GetEnvironmentVariable("AWS_JWT_ISSUER") ??  _configuration["AWS:Issuer"];
        } 
        
        public string GetAudience()
        {
            return Environment.GetEnvironmentVariable("AWS_JWT_AUDIENCE") ??  _configuration["AWS:Audience"];
        } 
        
        public int GetDurationInMinutes()
        {
            return Convert.ToInt16(Environment.GetEnvironmentVariable("AWS_JWT_VALID_MINUTES") ??  _configuration["AWS:DurationInMinutes"]);
        }

        public string GetRegion()
        {
            return Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION") ?? _configuration["AWS:Region"];
        }

        public string GetSecretKey()
        {
            return Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? _configuration["AWS:SecretKey"];
        }

        public string GetUserPoolClientId()
        {
            return Environment.GetEnvironmentVariable("COGNITO_APP_CLIENT_ID") ?? _configuration["AWS:UserPoolClientId"];
        }

        public string GetUserPoolClientSecret()
        {
            return Environment.GetEnvironmentVariable("COGNITO_APP_CLIENT_SECRET") ?? _configuration["AWS:UserPoolClientSecret"];
        }

        public string GetUserPoolId()
        {
            return Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID") ?? _configuration["AWS:UserPoolId"];
        }
    }
}