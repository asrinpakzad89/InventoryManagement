namespace Application.Common.Services;

public class TokenService : ITokenService
{
    private readonly JwtSetting _jwtSettings;

    public TokenService(IOptions<JwtSetting> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public TokenDto GenerateToken(UserDto user)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
            throw new Exception("JWT Key is not configured.");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Username),
        new Claim(ClaimTypes.Name, user.Id.ToString()),
    };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenDto
        {
            Token = tokenHandler.WriteToken(token)
        };
    }
}
