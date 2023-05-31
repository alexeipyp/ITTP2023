using BLL.Configs;
using BLL.Models.Token;
using Common.Consts;
using Common.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly AuthConfig _config;

        public AuthService(UserRepository userRepository, IOptions<AuthConfig> config)
        {
            _userRepository = userRepository;
            _config = config.Value;
        }

        public async Task<TokenModel> GetTokenByCredentialsAsync(string login, string password)
        {
            var user = await _userRepository.ReadActiveUserByCredentials(login, password);
            if (user == null)
            {
                throw new UnauthorizedException();
            }

            return GenerateTokens(user!);
        }

        public async Task<TokenModel> GetTokenByRefreshTokenAsync(string refreshToken)
        {
            var validParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = _config.SymmetricSecuriryKey(),
            };

            if (new JwtSecurityToken(refreshToken) == null)
            {
                throw new SecurityTokenException("invalid token");
            }

            var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken
                || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("invalid token");
            }

            if (principal.Claims.FirstOrDefault(x => x.Type == "guid")?.Value is String guidString
                && Guid.TryParse(guidString, out var guid))
            {
                var user = await _userRepository.ReadActiveUserByGuid(guid);
                if (user == null)
                {
                    throw new UnauthorizedException();
                }

                return GenerateTokens(user);
            }
            else
            {
                throw new SecurityTokenException("invalid token");
            }
        }

        private TokenModel GenerateTokens(DAL.Entities.User user)
        {
            var dtNow = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                notBefore: dtNow,
                claims: new Claim[] {
                    new Claim(ClaimNames.RequesterGuid, user.Guid.ToString()),
                    new Claim(ClaimNames.RequesterLogin, user.Login.ToString()),
                },
                expires: DateTime.Now.AddMinutes(_config.LifeTime),
                signingCredentials: new SigningCredentials(_config.SymmetricSecuriryKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refresh = new JwtSecurityToken(
                notBefore: dtNow,
                claims: new Claim[] {
                    new Claim(ClaimNames.RequesterGuid, user.Guid.ToString()),
                },
                expires: DateTime.Now.AddHours(_config.LifeTime),
                signingCredentials: new SigningCredentials(_config.SymmetricSecuriryKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedRefresh = new JwtSecurityTokenHandler().WriteToken(refresh);

            return new TokenModel
            {
                AccessToken = encodedJwt,
                RefreshToken = encodedRefresh
            };
        }

        public async Task<bool> IsUserActive(Guid userGuid)
            => await _userRepository.IsUserActive(userGuid);

    }
}
