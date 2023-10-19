using SportEvent.Models;

namespace SportEvent.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUser(LoginModel model);
        Task<bool> Register(RegisterModel model);
        Task<UserModel> GetProfile(HttpClient _apiClient);
        Task<bool> Edit(UserModel data, HttpClient _apiClient);
        Task<bool> ChangePassword(ChangePassword data, HttpClient _apiClient);
        Task<bool> Delete(HttpClient _apiClient);
    }
}
