using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Tests.Setup;

namespace Api.Tests.Auth;

[Collection("Api")]
public class LoginTests : ApiTestBase
{
    private const string Route = "/api/auth/login";

    public LoginTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_JuisteGegevens_Geeft200EnCookie()
    {
        // Arrange
        var client = Factory.MaakClient();
        var verzoek = new { email = PortfolioApiFactory.EditorEmail, password = Factory.Wachtwoord };

        // Act
        var response = await client.PostAsJsonAsync(Route, verzoek);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        Assert.Equal(PortfolioApiFactory.EditorEmail, body!.Email);
        Assert.Contains("Editor", body.Rollen);
        var cookie = Assert.Single(response.Headers.GetValues("Set-Cookie"));
        Assert.StartsWith(".AspNetCore.Identity.Application=", cookie);
        Assert.Contains("httponly", cookie, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(PortfolioApiFactory.GastEmail, "fout-wachtwoord")]
    [InlineData("onbekend@example.test", "abcdef")]
    public async Task Login_FouteGegevens_Geeft401ZonderCookie(string email, string password)
    {
        // Arrange
        var client = Factory.MaakClient();

        // Act
        var response = await client.PostAsJsonAsync(Route, new { email, password });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(response.Headers.Contains("Set-Cookie"));
    }

    [Theory]
    [InlineData("geen-email", "abcdef")]
    [InlineData("", "abcdef")]
    [InlineData(PortfolioApiFactory.GastEmail, "")]
    public async Task Login_OngeldigeInvoer_Geeft400(string email, string password)
    {
        // Arrange
        var client = Factory.MaakClient();

        // Act
        var response = await client.PostAsJsonAsync(Route, new { email, password });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
