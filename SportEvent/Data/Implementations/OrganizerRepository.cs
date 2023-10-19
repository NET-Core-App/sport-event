using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportEvent.Data.Implementations
{
    public class OrganizerRepository : IOrganizerRepository
    {
        public async Task<bool> Create(Organizer model, HttpClient _apiClient)
        {
            try
            {
                using var httpClient = new HttpClient();

                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PostAsync("organizers", content);

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

        public async Task<OrganizerResponse> GetAllOrganizers(HttpClient _apiClient, int page, int perPage )
        {
            try
            {
                using var response = await _apiClient.GetAsync($"organizers?page={page}&perPage={perPage}");

                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<OrganizerResponse>(responseData);
                return apiResponse;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<Organizer> GetById(int id, HttpClient _apiClient)
        {
            try
            {
                using var response = await _apiClient.GetAsync($"organizers/{id}");
                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Organizer>(responseData);

            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions (e.g., network issues)
                // Log the exception or return null based on your application's logic
                return null;
            }
        }

        public async Task<bool> EditOrganizer(int id, Organizer data, HttpClient _apiClient)
        {
            try
            {
                using var response = await _apiClient.PutAsync($"organizers/{id}", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

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


        public async Task<bool> Delete(int id, HttpClient _apiClient)
        {
            try
            {
                using var response = await _apiClient.DeleteAsync($"organizers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                return false;
            }

            return false;
        }
    }
}
