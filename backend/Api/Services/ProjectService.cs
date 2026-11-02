using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ProjectService
{
    private readonly PortfolioDbContext _db;

    public ProjectService(PortfolioDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProjectResponseDto>> GetAllAsync()
    {
        var projects = await _db.Projects.ToListAsync();
        return projects.Select(ToDto).ToList();
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return null;
        }
        return ToDto(project);
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectCreateDto dto)
    {
        var project = new Project
        {
            Titel = dto.Titel,
            Beschrijving = dto.Beschrijving,
            Categorie = dto.Categorie,
            GitHubUrl = dto.GitHubUrl,
            Datum = dto.Datum
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        return ToDto(project);
    }

    public async Task<bool> UpdateAsync(int id, ProjectUpdateDto dto)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }

        project.Titel = dto.Titel;
        project.Beschrijving = dto.Beschrijving;
        project.Categorie = dto.Categorie;
        project.GitHubUrl = dto.GitHubUrl;
        project.Datum = dto.Datum;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true;
    }

    // De enige plek waar een Project-entiteit wordt omgezet naar een DTO
    private static ProjectResponseDto ToDto(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Titel = project.Titel,
            Beschrijving = project.Beschrijving,
            Categorie = project.Categorie,
            GitHubUrl = project.GitHubUrl,
            Datum = project.Datum
        };
    }
}
