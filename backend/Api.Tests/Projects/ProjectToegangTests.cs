using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Projects;

[Collection("Api")]
public class ProjectToegangTests : ApiTestBase
{
    public ProjectToegangTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Schrijven_ZonderLogin_Geeft401EnWijzigtNiets(string methode)
    {
        // Arrange
        var project = await Factory.VoegToeAsync(ProjectTestData.Bestaand());
        var client = Factory.MaakClient();

        // Act
        var response = await StuurAsync(client, methode, project.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Project>(project.Id);
        Assert.Equal(project.Titel, opgeslagen!.Titel);
        Assert.Equal(1, await Factory.TelAsync<Project>());
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Schrijven_AlsGast_Geeft403EnWijzigtNiets(string methode)
    {
        // Arrange
        var project = await Factory.VoegToeAsync(ProjectTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.GastEmail);

        // Act
        var response = await StuurAsync(client, methode, project.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Project>(project.Id);
        Assert.Equal(project.Titel, opgeslagen!.Titel);
        Assert.Equal(1, await Factory.TelAsync<Project>());
    }

    private static Task<HttpResponseMessage> StuurAsync(HttpClient client, string methode, int id)
    {
        return methode switch
        {
            "POST" => client.PostAsJsonAsync(ProjectTestData.Route, ProjectTestData.Nieuw()),
            "PUT" => client.PutAsJsonAsync($"{ProjectTestData.Route}/{id}", ProjectTestData.Gewijzigd()),
            _ => client.DeleteAsync($"{ProjectTestData.Route}/{id}")
        };
    }
}
