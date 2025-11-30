namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResultDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
