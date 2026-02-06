using FEMEE.Application.DTOs.Auth;
using FEMEE.Application.Interfaces.Common;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Services.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para autenticação.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("auth")] // Aplica rate limiting mais restritivo para auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthenticationService authService,
            IUserService userService,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator,
            IPasswordHasher passwordHasher,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Faz login de um usuário.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                    return BadRequest("Email e senha são obrigatórios");

                var user = await _userService.GetUserEntityByEmailAsync(request.Email);

                if (string.IsNullOrWhiteSpace(user?.Senha))
                    return Unauthorized("Email ou senha inválidos");

                if (!_passwordHasher.VerifyPassword(request.Senha, user.Senha))
                    return Unauthorized("Email ou senha inválidos");

                var token = await _authService.GenerateTokenAsync(user);

                // Calcula expiração baseado na configuração (60 minutos por padrão)
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                return Ok(new LoginResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    Nome = user.Nome,
                    TipoUsuario = user.TipoUsuario,
                    ExpiresAt = expiresAt
                });
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized("Email ou senha inválidos");
            }
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var createUserDto = new FEMEE.Application.DTOs.User.CreateUserDto
                {
                    Nome = request.Nome ?? string.Empty,
                    Email = request.Email ?? string.Empty,
                    Senha = request.Senha ?? string.Empty,
                    Telefone = request.Telefone ?? string.Empty,
                    TipoUsuario = FEMEE.Domain.Enums.TipoUsuario.Jogador
                };

                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetCurrentUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém o usuário atual autenticado.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound();

            return Ok(user);
        }
    }
}
