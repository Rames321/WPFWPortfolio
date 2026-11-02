using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _service;

    public ProjectsController(ProjectService service)
    {
        _service = service;
    }

    // GET /api/projects
    [HttpGet]
    public async Task<ActionResult<List<ProjectResponseDto>>> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects);
    }

    // GET /api/projects/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    // POST /api/projects
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(ProjectCreateDto dto)
    {
        var project = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    // PUT /api/projects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProjectUpdateDto dto)
    {
        var gelukt = await _service.UpdateAsync(id, dto);
        if (!gelukt)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE /api/projects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var gelukt = await _service.DeleteAsync(id);
        if (!gelukt)
        {
            return NotFound();
        }
        return NoContent();
    }
}
