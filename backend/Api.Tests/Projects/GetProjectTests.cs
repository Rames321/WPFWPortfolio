using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Tests.Setup;

namespace Api.Tests.Projects;

[Collection("Api")]
public class GetProjectTests : ApiTestBase
{
    public GetProjectTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ZonderLogin_GeeftAlleProjecten()
    {
        // Arrange
        await Factory.VoegToeAsync(ProjectTestData.Bestaand("Eerste"));
        await Factory.VoegToeAsync(ProjectTestData.Bestaand("Tweede"));
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync(ProjectTestData.Route);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var projecten = await response.Content.ReadFromJsonAsync<List<ProjectResponseDto>>();
        Assert.Equal(2, projecten!.Count);
        Assert.Contains(projecten, p => p.Titel == "Eerste");
        Assert.Contains(projecten, p => p.Titel == "Tweede");
    }

    [Fact]
    public async Task GetById_BestaandId_GeeftProject()
    {
        // Arrange
        var project = await Factory.VoegToeAsync(ProjectTestData.Bestaand());
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync($"{ProjectTestData.Route}/{project.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseDto>();
        Assert.Equal(project.Id, body!.Id);
        Assert.Equal(project.Titel, body.Titel);
        Assert.Equal(project.GitHubUrl, body.GitHubUrl);
    }

    [Fact]
    public async Task GetById_OnbekendId_Geeft404()
    {
        // Arrange
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync($"{ProjectTestData.Route}/{ProjectTestData.OnbekendId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
