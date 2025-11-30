using Application.Common.ViewModels;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    TokenDto GenerateToken(UserDto user);
}
