using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Blogposts;

[Collection("Api")]
public class BlogpostToegangTests : ApiTestBase
{
    public BlogpostToegangTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Schrijven_ZonderLogin_Geeft401EnWijzigtNiets(string methode)
    {
        // Arrange
        var blogpost = await Factory.VoegToeAsync(BlogpostTestData.Bestaand());
        var client = Factory.MaakClient();

        // Act
        var response = await StuurAsync(client, methode, blogpost.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Blogpost>(blogpost.Id);
        Assert.Equal(blogpost.Titel, opgeslagen!.Titel);
        Assert.Equal(1, await Factory.TelAsync<Blogpost>());
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Schrijven_AlsGast_Geeft403EnWijzigtNiets(string methode)
    {
        // Arrange
        var blogpost = await Factory.VoegToeAsync(BlogpostTestData.Bestaand());
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.GastEmail);

        // Act
        var response = await StuurAsync(client, methode, blogpost.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var opgeslagen = await Factory.ZoekAsync<Blogpost>(blogpost.Id);
        Assert.Equal(blogpost.Titel, opgeslagen!.Titel);
        Assert.Equal(1, await Factory.TelAsync<Blogpost>());
    }

    private static Task<HttpResponseMessage> StuurAsync(HttpClient client, string methode, int id)
    {
        return methode switch
        {
            "POST" => client.PostAsJsonAsync(BlogpostTestData.Route, BlogpostTestData.Nieuw()),
            "PUT" => client.PutAsJsonAsync($"{BlogpostTestData.Route}/{id}", BlogpostTestData.Gewijzigd()),
            _ => client.DeleteAsync($"{BlogpostTestData.Route}/{id}")
        };
    }
}
