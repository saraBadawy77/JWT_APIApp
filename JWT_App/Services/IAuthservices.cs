using JWT_App.Models;

namespace JWT_App.Services
{
    public interface IAuthservices
    {
        Task<AuthantationModel> RegisterAsync(RegistrationModel model);

        Task<AuthantationModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(AddRoleModel model);


    }
}
