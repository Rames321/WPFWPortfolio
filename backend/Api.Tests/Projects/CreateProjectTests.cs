using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Projects;

[Collection("Api")]
public class CreateProjectTests : ApiTestBase
{
    public CreateProjectTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Create_AlsEditorOfAdmin_SlaatProjectOp(string email)
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(email);
        var dto = ProjectTestData.Nieuw();

        // Act
        var response = await client.PostAsJsonAsync(ProjectTestData.Route, dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseDto>();
        var opgeslagen = await Factory.ZoekAsync<Project>(body!.Id);
        Assert.Equal(dto.Titel, body.Titel);
        Assert.Equal(dto.Titel, opgeslagen!.Titel);
        Assert.Equal(dto.GitHubUrl, opgeslagen.GitHubUrl);
    }

    [Theory]
    [InlineData("", ProjectTestData.GeldigeUrl, 2026)]
    [InlineData("Titel", "geen-url", 2026)]
    [InlineData("Titel", ProjectTestData.GeldigeUrl, 1999)]
    public async Task Create_OngeldigeInvoer_Geeft400EnSlaatNietsOp(string titel, string url, int jaar)
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);
        var dto = ProjectTestData.Nieuw();
        dto.Titel = titel;
        dto.GitHubUrl = url;
        dto.Datum = new DateTime(jaar, 1, 15);

        // Act
        var response = await client.PostAsJsonAsync(ProjectTestData.Route, dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, await Factory.TelAsync<Project>());
    }
}
