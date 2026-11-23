using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Tests.Setup;

namespace Api.Tests.Blogposts;

[Collection("Api")]
public class GetBlogpostTests : ApiTestBase
{
    public GetBlogpostTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ZonderLogin_GeeftAlleBlogposts()
    {
        // Arrange
        await Factory.VoegToeAsync(BlogpostTestData.Bestaand("Eerste"));
        await Factory.VoegToeAsync(BlogpostTestData.Bestaand("Tweede"));
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync(BlogpostTestData.Route);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var blogposts = await response.Content.ReadFromJsonAsync<List<BlogpostResponseDto>>();
        Assert.Equal(2, blogposts!.Count);
        Assert.Contains(blogposts, b => b.Titel == "Eerste");
        Assert.Contains(blogposts, b => b.Titel == "Tweede");
    }

    [Fact]
    public async Task GetById_BestaandId_GeeftBlogpost()
    {
        // Arrange
        var blogpost = await Factory.VoegToeAsync(BlogpostTestData.Bestaand());
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync($"{BlogpostTestData.Route}/{blogpost.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BlogpostResponseDto>();
        Assert.Equal(blogpost.Id, body!.Id);
        Assert.Equal(blogpost.Titel, body.Titel);
        Assert.Equal(blogpost.Inhoud, body.Inhoud);
    }

    [Fact]
    public async Task GetById_OnbekendId_Geeft404()
    {
        // Arrange
        var client = Factory.MaakClient();

        // Act
        var response = await client.GetAsync($"{BlogpostTestData.Route}/{BlogpostTestData.OnbekendId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
