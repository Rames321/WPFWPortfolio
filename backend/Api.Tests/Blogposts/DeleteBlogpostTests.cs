using System.Net;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Blogposts;

[Collection("Api")]
public class DeleteBlogpostTests : ApiTestBase
{
    public DeleteBlogpostTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Delete_AlsEditorOfAdmin_VerwijdertBlogpost(string email)
    {
        // Arrange
        var blogpost = await Factory.VoegToeAsync(BlogpostTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(email);

        // Act
        var response = await client.DeleteAsync($"{BlogpostTestData.Route}/{blogpost.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(await Factory.ZoekAsync<Blogpost>(blogpost.Id));
    }

    [Fact]
    public async Task Delete_OnbekendId_Geeft404()
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);

        // Act
        var response = await client.DeleteAsync($"{BlogpostTestData.Route}/{BlogpostTestData.OnbekendId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
