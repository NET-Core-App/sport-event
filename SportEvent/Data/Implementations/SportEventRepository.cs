using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace SportEvent.Data.Implementations
{
    public class SportEventRepository : ISportEventRepository
    {
        public async Task<bool> Create(SportEventCreateModel model, HttpClient _apiClient)
        {
            try
            {
                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PostAsync("sport-events", content);

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

        public async Task<bool> Delete(int id, HttpClient _apiClient)
        {

            try
            {
                using var response = await _apiClient.DeleteAsync($"sport-events/{id}");

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

        public async Task<bool> Edit(int id, SportEventCreateModel model, HttpClient _apiClient)
        {
            try
            {
                StringContent content = new(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using var response = await _apiClient.PutAsync($"sport-events/{id}", content);

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

        public async Task<SportEventResponse> GetAll(HttpClient _apiClient, int page, int perPage)
        {
            try
            {
                using var response = await _apiClient.GetAsync($"sport-events?page={page}&perPage={perPage}");
                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<SportEventResponse>(responseData);

                return apiResponse;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }

        public async Task<SportEventCreateModel> GetById(int id, HttpClient _apiClient)
        {
            try
            {
                using var response = await _apiClient.GetAsync($"sport-events/{id}");
                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SportEventCreateModel>(responseData);

            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }


        public async Task<SportEventModel> Detail(int id, HttpClient _apiClient)
        {
            try
            {
                using var response = await _apiClient.GetAsync($"sport-events/{id}");
                response.EnsureSuccessStatusCode(); // Throw an exception for non-success status codes

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SportEventModel>(responseData);

            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error occurred while making the HTTP request.", ex);
            }
        }
    }
}
