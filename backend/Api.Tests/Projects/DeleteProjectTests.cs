using System.Net;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Projects;

[Collection("Api")]
public class DeleteProjectTests : ApiTestBase
{
    public DeleteProjectTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Delete_AlsEditorOfAdmin_VerwijdertProject(string email)
    {
        // Arrange
        var project = await Factory.VoegToeAsync(ProjectTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(email);

        // Act
        var response = await client.DeleteAsync($"{ProjectTestData.Route}/{project.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(await Factory.ZoekAsync<Project>(project.Id));
    }

    [Fact]
    public async Task Delete_OnbekendId_Geeft404()
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);

        // Act
        var response = await client.DeleteAsync($"{ProjectTestData.Route}/{ProjectTestData.OnbekendId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
