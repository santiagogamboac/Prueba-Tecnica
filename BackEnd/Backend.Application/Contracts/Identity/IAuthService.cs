using Backend.Application.Models.Common;
using Backend.Application.Models.Identity;

namespace Backend.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<AuthResponse> RefreshToken(TokenRequest request);    
        (string, Guid)? ValidateToken(string token);
        Task<GeneralResponse> Register(RegistrationRequest request);
        Task<List<object>> GetAllUser(GetUsersParams @params);
        Task<object> GetUserById(Guid userId);
        Task<List<object>> GetUsersByIds(List<Guid> userIds);
        //Task<List<Role>> GetRoles();
        Task<string> GetUserName();
        Task<string> UpdateUser(Guid userId, UpdateUserRequest updatedUser);
    }
}
