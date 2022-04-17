namespace GISA.Commons.SDK.AWS
{
    public interface ICloudConfigurationService
    {
        string GetAccessKey();
        public string GetKey();
        public string GetIssuer();
        public string GetAudience();
        public int GetDurationInMinutes();
        string GetSecretKey();
        string GetRegion();
        string GetUserPoolClientId();
        string GetUserPoolId();
        string GetUserPoolClientSecret();
    }
}