using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;

namespace dkt_learn_core.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenResponseDto request);
    Task<bool> LogoutAsync(Guid userId);
}