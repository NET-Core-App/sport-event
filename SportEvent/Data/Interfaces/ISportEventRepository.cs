using SportEvent.Models;

namespace SportEvent.Data.Interfaces
{
    public interface ISportEventRepository
    {
        Task<bool> Create(SportEventCreateModel model, HttpClient _apiClient);
        Task<SportEventResponse> GetAll(HttpClient _apiClient, int page, int perPage);
        Task<SportEventCreateModel> GetById(int id, HttpClient _apiClient);
        Task<SportEventModel> Detail(int id, HttpClient _apiClient);
        Task<bool> Edit(int id, SportEventCreateModel model, HttpClient _apiClient);
        Task<bool> Delete(int id, HttpClient _apiClient);
    }
}
