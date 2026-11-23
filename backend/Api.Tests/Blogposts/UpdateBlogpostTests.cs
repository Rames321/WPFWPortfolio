using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Blogposts;

[Collection("Api")]
public class UpdateBlogpostTests : ApiTestBase
{
    public UpdateBlogpostTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Update_AlsEditorOfAdmin_WijzigtBlogpost(string email)
    {
        // Arrange
        var blogpost = await Factory.VoegToeAsync(BlogpostTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(email);
        var dto = BlogpostTestData.Gewijzigd();

        // Act
        var response = await client.PutAsJsonAsync($"{BlogpostTestData.Route}/{blogpost.Id}", dto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Blogpost>(blogpost.Id);
        Assert.Equal(dto.Titel, opgeslagen!.Titel);
        Assert.Equal(dto.Inhoud, opgeslagen.Inhoud);
    }

    [Fact]
    public async Task Update_OnbekendId_Geeft404()
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);

        // Act
        var response = await client.PutAsJsonAsync(
            $"{BlogpostTestData.Route}/{BlogpostTestData.OnbekendId}",
            BlogpostTestData.Gewijzigd());

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
