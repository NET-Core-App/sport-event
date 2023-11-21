using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace SportEvent.Data.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _configure;

        public UserRepository(IHttpContextAccessor httpContextAccessor, IConfiguration configure)
        {
            _httpContextAccessor = httpContextAccessor;
            _configure = configure["APIUrl:BaseAddress"];
            _apiClient = new HttpClient { BaseAddress = new Uri(_configure) };
        }

        public async Task<bool> AuthenticateUser(LoginModel model)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PostAsync("users/login", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    // Store the token in session
                    _httpContextAccessor.HttpContext.Session.SetString("AccessToken", token);
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", model.id);
                    _httpContextAccessor.HttpContext.Session.SetString("IsAuthenticated", "true");
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", loginResponse.id);

                    return true;
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }

        public async Task<bool> Delete(HttpClient apiClient)
        {
            try
            {
                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.DeleteAsync($"users/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }

        }

        public async Task<bool> Edit(UserModel model, HttpClient apiClient)
        {
            try
            {
                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.PutAsync($"users/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }

        public async Task<UserModel> GetProfile(HttpClient apiClient)
        {
            try
            {
                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.GetAsync($"users/{userId}");

                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(responseData);

            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }

        public async Task<bool> Register(RegisterModel model)
        {
            try
            {
                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PostAsync("users", content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }
        public async Task<bool> ChangePassword(ChangePassword model, HttpClient apiClient)
        {
            try
            {
                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.PutAsync($"users/{userId}/password", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }
/*
        private string GetAccessTokenFromSession()
        {
            // Replace this with your actual code to retrieve the access token from the session
            // For example, in ASP.NET, you might use HttpContext.Session
            // In other frameworks or technologies, the approach might be different
            // This is just a placeholder example
            return _httpContextAccessor.HttpContext.Session.GetString("AccessToken");
        }

        public bool IsAuthorization()
        {
            var accessToken = (string)JObject.Parse(GetAccessTokenFromSession())["token"];

            if (string.IsNullOrEmpty(accessToken))
            {
                // Handle the case where the access token is not available
                // You may want to throw an exception or handle it based on your application's logic
                return false;
            }

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            return true;
        }
*/
    
    }
}
