using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Projects;

[Collection("Api")]
public class UpdateProjectTests : ApiTestBase
{
    public UpdateProjectTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Update_AlsEditorOfAdmin_WijzigtProject(string email)
    {
        // Arrange
        var project = await Factory.VoegToeAsync(ProjectTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(email);
        var dto = ProjectTestData.Gewijzigd();

        // Act
        var response = await client.PutAsJsonAsync($"{ProjectTestData.Route}/{project.Id}", dto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Project>(project.Id);
        Assert.Equal(dto.Titel, opgeslagen!.Titel);
        Assert.Equal(dto.GitHubUrl, opgeslagen.GitHubUrl);
    }

    [Fact]
    public async Task Update_OnbekendId_Geeft404()
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);

        // Act
        var response = await client.PutAsJsonAsync(
            $"{ProjectTestData.Route}/{ProjectTestData.OnbekendId}",
            ProjectTestData.Gewijzigd());

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
