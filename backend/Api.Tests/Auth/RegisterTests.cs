using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Models;
using Api.Tests.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Auth;

[Collection("Api")]
public class RegisterTests : ApiTestBase
{
    private const string Route = "/api/auth/register";

    public RegisterTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_GeldigeGegevens_GeeftRolGastEnHashtWachtwoord()
    {
        // Arrange
        var client = Factory.MaakClient();
        var email = $"{Guid.NewGuid():N}@example.test";

        // Act
        var response = await client.PostAsJsonAsync(Route, new { email, password = Factory.Wachtwoord });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        Assert.Equal(email, body!.Email);
        Assert.Equal(new[] { "Gast" }, body.Rollen);

        using var scope = Factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(email);
        Assert.NotNull(user!.PasswordHash);
        Assert.NotEqual(Factory.Wachtwoord, user.PasswordHash);
    }

    [Fact]
    public async Task Register_MetAdminRolInVerzoek_BlijftGast()
    {
        // Arrange
        var client = Factory.MaakClient();
        var email = $"{Guid.NewGuid():N}@example.test";
        var verzoek = new { email, password = Factory.Wachtwoord, rol = "Admin", rollen = new[] { "Admin" } };

        // Act
        var response = await client.PostAsJsonAsync(Route, verzoek);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        Assert.Equal(new[] { "Gast" }, body!.Rollen);
    }

    [Theory]
    [InlineData("geen-email", "abcdef")]
    [InlineData("nieuw@example.test", "")]
    [InlineData("nieuw@example.test", "abcde")]
    [InlineData(PortfolioApiFactory.GastEmail, "abcdef")]
    public async Task Register_OngeldigeInvoer_Geeft400EnMaaktGeenAccount(string email, string password)
    {
        // Arrange
        var client = Factory.MaakClient();
        var aantalVoor = await Factory.TelAsync<User>();

        // Act
        var response = await client.PostAsJsonAsync(Route, new { email, password });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(aantalVoor, await Factory.TelAsync<User>());
    }
}
