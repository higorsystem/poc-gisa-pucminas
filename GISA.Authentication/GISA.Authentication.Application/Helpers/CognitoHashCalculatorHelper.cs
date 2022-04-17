using System;
using System.Security.Cryptography;
using System.Text;

namespace GISA.Authentication.Application.Helpers
{
    public static class CognitoHashCalculatorHelper
    {
        /// <summary>
        /// Computes the secret hash for the user pool using the corresponding userID, clientID,
        /// and client secret
        /// </summary>
        /// <param name="userName">The current userName</param>
        /// <param name="clientId">The clientId for the client being used</param>
        /// <param name="clientSecret">The client secret of the corresponding clientId</param>
        /// <returns>Returns the secret hash for the user pool using the corresponding
        /// userID, clientID, and client secret</returns>
        public static string GetUserPoolSecretHash(string userName, string clientId, string clientSecret)
        {
            string message = userName + clientId;
            byte[] keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            HMACSHA256 hmacSha256 = new HMACSHA256(keyBytes);
            byte[] hashMessage = hmacSha256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashMessage);
        }
    }
}