using Application.Features.Auth.Commands.Login;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.NameIdentifier, result.UserDetailDto.Username),
                  new Claim(ClaimTypes.Name, result.UserDetailDto.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10)
            });

            return Json(new
            {
                isSuccess = true,
                token = result.Token
            });
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }

}
