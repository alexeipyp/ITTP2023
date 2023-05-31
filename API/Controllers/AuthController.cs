using API.Models.Token;
using BLL.Models.Token;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService = null!;

        public AuthController(AuthService authService)
        {
            _authService= authService;
        }

        [HttpPost]
        public async Task<TokenModel> TokenByCredentials(TokenByCredentialsRequest request)
            => await _authService.GetTokenByCredentialsAsync(request.Login, request.Password);

        [HttpPost]
        public async Task<TokenModel> TokenByRefreshToken(TokenByRefreshTokenRequest request)
            => await _authService.GetTokenByRefreshTokenAsync(request.RefreshToken);
    }
}
