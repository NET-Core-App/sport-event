using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SportEvent.Models;
using System.Net.Http.Headers;

namespace SportEvent.Data
{
    public class AuthorizationService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _configure;

        public AuthorizationService(IHttpContextAccessor httpContextAccessor, IConfiguration configure)
        {
            _httpContextAccessor = httpContextAccessor;
            _configure = configure["APIUrl:BaseAddress"];
        
        }

        private string GetAccessTokenFromSession()
        {
            string accessToken = _httpContextAccessor.HttpContext.Session.GetString("AccessToken");

            // Check if the access token is null or empty
            if (string.IsNullOrEmpty(accessToken))
            {
                // You might want to log a message or handle the situation accordingly.
                // Returning null or an empty string is an option.
                return null;
            }

            return accessToken;
        }

        public bool IsAuthorization(HttpClient _apiClient)
        {
            var accessTokenString = GetAccessTokenFromSession();

            if (string.IsNullOrEmpty(accessTokenString))
            {
                return false;
            }

            var accessToken = (string)JObject.Parse(accessTokenString)["token"];

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            // Set the BaseAddress and Authorization header if needed
            _apiClient.BaseAddress = new Uri(_configure); // Assuming _configure is properly defined
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return true;
        }

    }
}
