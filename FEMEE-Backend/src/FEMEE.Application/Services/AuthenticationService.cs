using FEMEE.Domain.Entities.Principal;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FEMEE.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRepository<User> _userRepository;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings, IRepository<User> userRepository)
        {
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Usuário deve possuir e-mail.", nameof(user));

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey ?? string.Empty);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.Nome ?? string.Empty),
                    new Claim("TipoUsuario", user.TipoUsuario.ToString()),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                   Subject = new ClaimsIdentity(claims),
                   Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                   Issuer = _jwtSettings.Issuer ?? string.Empty,
                   Audience = _jwtSettings.Audience ?? string.Empty,
                   SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return await Task.FromResult(tokenString);


            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao gerar token JWT.", ex);
            }
        }
        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return await Task.FromResult(false);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
                var validateionParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validateionParameters, out SecurityToken validatedToken);
                return await Task.FromResult(true);
            }
            catch (SecurityTokenException)
            {
                return await Task.FromResult(false);

            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<User?> GetUserFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            if (!await ValidateTokenAsync(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return null;

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                    return null;

                var user = await _userRepository.GetByIdAsync(userId);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> GetUserIdFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return -1;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    return -1;
                }
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                    return -1;

                return await Task.FromResult(userId);

            }
            catch (Exception)
            {
                return -1;
            }

        }

    }
}
