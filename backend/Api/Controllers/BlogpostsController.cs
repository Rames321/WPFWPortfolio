using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/blogposts")]
public class BlogpostsController : ControllerBase
{
    private readonly BlogpostService _service;

    public BlogpostsController(BlogpostService service)
    {
        _service = service;
    }

    // GET /api/blogposts
    [HttpGet]
    public async Task<ActionResult<List<BlogpostResponseDto>>> GetAll()
    {
        var blogposts = await _service.GetAllAsync();
        return Ok(blogposts);
    }

    // GET /api/blogposts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogpostResponseDto>> GetById(int id)
    {
        var blogpost = await _service.GetByIdAsync(id);
        if (blogpost == null)
        {
            return NotFound();
        }
        return Ok(blogpost);
    }

    // POST /api/blogposts
    [HttpPost]
    public async Task<ActionResult<BlogpostResponseDto>> Create(BlogpostCreateDto dto)
    {
        var blogpost = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = blogpost.Id }, blogpost);
    }

    // PUT /api/blogposts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BlogpostUpdateDto dto)
    {
        var gelukt = await _service.UpdateAsync(id, dto);
        if (!gelukt)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE /api/blogposts/5
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
