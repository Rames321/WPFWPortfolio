using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Models;
using Api.Tests.Setup;

namespace Api.Tests.Blogposts;

[Collection("Api")]
public class CreateBlogpostTests : ApiTestBase
{
    public CreateBlogpostTests(PortfolioApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(PortfolioApiFactory.EditorEmail)]
    [InlineData(PortfolioApiFactory.AdminEmail)]
    public async Task Create_AlsEditorOfAdmin_SlaatBlogpostOp(string email)
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(email);
        var dto = BlogpostTestData.Nieuw();

        // Act
        var response = await client.PostAsJsonAsync(BlogpostTestData.Route, dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BlogpostResponseDto>();
        var opgeslagen = await Factory.ZoekAsync<Blogpost>(body!.Id);
        Assert.Equal(dto.Titel, body.Titel);
        Assert.Equal(dto.Titel, opgeslagen!.Titel);
        Assert.Equal(dto.Inhoud, opgeslagen.Inhoud);
    }

    [Theory]
    [InlineData("", "Inhoud", 2026)]
    [InlineData("Titel", "", 2026)]
    [InlineData("Titel", "Inhoud", 1999)]
    public async Task Create_OngeldigeInvoer_Geeft400EnSlaatNietsOp(string titel, string inhoud, int jaar)
    {
        // Arrange
        var client = await Factory.LoginAlsAsync(PortfolioApiFactory.EditorEmail);
        var dto = BlogpostTestData.Nieuw();
        dto.Titel = titel;
        dto.Inhoud = inhoud;
        dto.Publicatiedatum = new DateTime(jaar, 1, 15);

        // Act
        var response = await client.PostAsJsonAsync(BlogpostTestData.Route, dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, await Factory.TelAsync<Blogpost>());
    }
}
