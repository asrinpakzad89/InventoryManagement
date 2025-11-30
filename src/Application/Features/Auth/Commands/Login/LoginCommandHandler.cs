using Application.Common.Interfaces;
using Application.Common.ViewModels;
using Framework.Exceptions;
using System.Data;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly ITokenService _tokenService;

    private readonly IEFRepository<User> _repository;

    private LoginCommand _command;
    public LoginCommandHandler(ITokenService tokenService, IEFRepository<User> repository)
    {
        _repository = repository;
        _tokenService = tokenService;
        _command = new();
    }

    public async Task<LoginResultDto> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        _command = command;
        _command.Username = _command.Username.ToLower();
        var userDto = await LoginUser(cancellationToken);

        TokenDto? token = null;
        token = CreateToken(userDto);

        return new LoginResultDto
        {
            UserDetailDto = userDto,
            Token = token
        };
    }
    private async Task<UserDto> LoginUser(CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(cancellationToken);

        var user = users.Where(x => x.UserName == _command.Username || x.Password == _command.Password)
           .FirstOrDefault();

        if (user == null)
        {
            throw new NotFoundException("کاربری با این مشخصات یافت نشد.");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName
        };
    }
    private TokenDto CreateToken(UserDto userDto)
    {
        return _tokenService.GenerateToken(new UserDto
        {
            Id = userDto.Id,
            Username = userDto.Username,
        });
    }
}
