using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Net.Http.Headers;
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
            catch (Exception ex)
            {
                throw;
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
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions (e.g., network issues)
                // Log the exception or handle it based on your application's logic
                return false;
            }

            return false;
        }

        public async Task<bool> Edit(UserModel data, HttpClient apiClient)
        {
            try
            {
                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.PutAsync($"users/{userId}", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Handle error response, log it, or throw an exception based on your application's logic
                    Console.WriteLine($"Error: {response.StatusCode}, {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or throw a custom exception based on your application's logic
                Console.WriteLine($"Error: {ex.Message}");
                return false;
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
                // Handle HTTP request exceptions (e.g., network issues)
                // Log the exception or return null based on your application's logic
                return null;
            }
        }

        public async Task<bool> Register(RegisterModel model)
        {
            try
            {
                using var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PostAsync("users", content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    /*       var content = await response.Content.ReadAsStringAsync();*/
                    // Handle error response, log it, or throw an exception based on your application's logic
                    Console.WriteLine($"Error: {response.StatusCode}, {content}");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ChangePassword(ChangePassword data, HttpClient apiClient)
        {
            try
            {
                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                using var response = await apiClient.PutAsync($"users/{userId}/password", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Handle error response, log it, or throw an exception based on your application's logic
                    Console.WriteLine($"Error: {response.StatusCode}, {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or throw a custom exception based on your application's logic
                Console.WriteLine($"Error: {ex.Message}");
                return false;
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
