using Api.Dtos;
using Api.Models;

namespace Api.Tests.Projects;

public static class ProjectTestData
{
    public const string Route = "/api/projects";
    public const string GeldigeUrl = "https://github.com/test/project";
    public const int OnbekendId = 999999;

    public static Project Bestaand(string titel = "Bestaand")
    {
        return new Project
        {
            Titel = titel,
            Beschrijving = "Beschrijving",
            Categorie = "Web",
            GitHubUrl = GeldigeUrl,
            Datum = new DateTime(2026, 1, 15)
        };
    }

    public static ProjectCreateDto Nieuw()
    {
        return new ProjectCreateDto
        {
            Titel = "Nieuw project",
            Beschrijving = "Nieuwe beschrijving",
            Categorie = "Web",
            GitHubUrl = GeldigeUrl,
            Datum = new DateTime(2026, 2, 20)
        };
    }

    public static ProjectUpdateDto Gewijzigd()
    {
        return new ProjectUpdateDto
        {
            Titel = "Gewijzigd",
            Beschrijving = "Gewijzigde beschrijving",
            Categorie = "Backend",
            GitHubUrl = "https://github.com/test/gewijzigd",
            Datum = new DateTime(2026, 3, 25)
        };
    }
}
