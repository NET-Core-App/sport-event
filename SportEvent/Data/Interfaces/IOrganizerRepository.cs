using SportEvent.Models;

namespace SportEvent.Data.Interfaces
{
    public interface IOrganizerRepository
    {
        Task<bool> Create(Organizer model, HttpClient _apiClient);
        Task<OrganizerResponse> GetAllOrganizers(HttpClient _apiClient, int page, int perPage);
        Task<Organizer> GetById(int id, HttpClient _apiClient);
        Task<bool> EditOrganizer(int id, Organizer data, HttpClient _apiClient);
        Task<bool> Delete(int id, HttpClient _apiClient);

    }
}
