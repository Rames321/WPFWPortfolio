using System.Net;
using System.Net.Http.Json;
using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

[Collection("Api")]
public class SetupTests
{
    private readonly PortfolioApiFactory _factory;

    public SetupTests(PortfolioApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Database_TijdensTests_IsDeTestDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();

        var database = db.Database.GetDbConnection().Database;

        Assert.Equal(PortfolioApiFactory.TestDatabase, database);
    }

    [Theory]
    [InlineData(PortfolioApiFactory.GastEmail, "Gast", false)]
    [InlineData(PortfolioApiFactory.EditorEmail, "Editor", true)]
    [InlineData(PortfolioApiFactory.AdminEmail, "Admin", true)]
    public async Task Testgebruiker_NaAanmaken_HeeftJuisteRol(string email, string rol, bool magSchrijven)
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(email);

        var rollen = await userManager.GetRolesAsync(user!);

        Assert.Contains(rol, rollen);
        Assert.Equal(magSchrijven, rollen.Contains("Editor") || rollen.Contains("Admin"));
    }

    [Fact]
    public async Task GetProjects_ZonderLoginEnZonderData_GeeftLegeLijst()
    {
        await _factory.LeegDataAsync();
        var client = _factory.MaakClient();

        var response = await client.GetAsync("/api/projects");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var projecten = await response.Content.ReadFromJsonAsync<List<ProjectResponseDto>>();
        Assert.Empty(projecten!);
    }
}
