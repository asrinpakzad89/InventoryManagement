namespace Application.Common.ViewModels.Login;

public class LoginResultDto
{
    public UserDto UserDetailDto { get; set; }
    public TokenDto? Token { get; set; }
}
