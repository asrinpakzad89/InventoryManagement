using Framework.Encryption;
using GishnizApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GishnizApp.Application.Common.Services;

public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public int GetUserId()
    {
        if (_httpContextAccessor?.HttpContext != null)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return 0;
            }
            return int.Parse(Crypt.Decrypt(userId));
        }
        return 0;
    }
}
