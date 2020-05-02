using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace Fitments_2
{
    public class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AWi9xPeH8Lf9h5UH_mVHAwaIjPvPGAR5E-PSXMZuEIL_py5gh_8U92QoXyxX_jyxX9YVkEb64okyegNY";
            ClientSecret = "EEF67IOF411zD4vG1JFrWMtZ11Q0MMKSCVd14287Nxqn-sfwCBp0SlmqL44DLh0XRLWZvOxn68kvK5-f";
        }
        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}