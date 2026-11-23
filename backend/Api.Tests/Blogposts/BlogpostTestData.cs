using Api.Dtos;
using Api.Models;

namespace Api.Tests.Blogposts;

public static class BlogpostTestData
{
    public const string Route = "/api/blogposts";
    public const int OnbekendId = 999999;

    public static Blogpost Bestaand(string titel = "Bestaand")
    {
        return new Blogpost
        {
            Titel = titel,
            Inhoud = "Inhoud",
            Publicatiedatum = new DateTime(2026, 1, 15)
        };
    }

    public static BlogpostCreateDto Nieuw()
    {
        return new BlogpostCreateDto
        {
            Titel = "Nieuwe blogpost",
            Inhoud = "Nieuwe inhoud",
            Publicatiedatum = new DateTime(2026, 2, 20)
        };
    }

    public static BlogpostUpdateDto Gewijzigd()
    {
        return new BlogpostUpdateDto
        {
            Titel = "Gewijzigd",
            Inhoud = "Gewijzigde inhoud",
            Publicatiedatum = new DateTime(2026, 3, 25)
        };
    }
}
