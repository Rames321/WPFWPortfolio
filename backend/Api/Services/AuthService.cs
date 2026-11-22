using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return result;
        }

        var rolResult = await _userManager.AddToRoleAsync(user, "Gast");
        if (!rolResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
        }
        return rolResult;
    }

    public async Task<bool> LoginAsync(LoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            dto.Email,
            dto.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task<UserResponseDto?> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var rollen = await _userManager.GetRolesAsync(user);
        return new UserResponseDto
        {
            Email = user.Email ?? "",
            Rollen = rollen.ToList()
        };
    }
}
