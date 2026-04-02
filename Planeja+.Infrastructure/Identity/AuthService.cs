using Microsoft.AspNetCore.Identity;
using Planeja_.Application.Abstractions;
using Planeja_.Application.DTOs.Auth;
using Planeja_.Application.Exceptions;
using Planeja_.Application.Services;

namespace Planeja_.Infrastructure.Identity;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new AuthException("An account with this email already exists.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new AuthException(errors);
        }

        var (token, expiresAt) = _tokenService.GenerateAccessToken(user.Id, user.Email!);
        return new AuthResponse(token, expiresAt);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new AuthException("Invalid email or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            throw new AuthException("Account is temporarily locked. Try again later.");

        if (!result.Succeeded)
            throw new AuthException("Invalid email or password.");

        var (token, expiresAt) = _tokenService.GenerateAccessToken(user.Id, user.Email!);
        return new AuthResponse(token, expiresAt);
    }
}
