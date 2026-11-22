using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class ProjectUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Titel { get; set; } = "";

    [Required]
    [StringLength(2000)]
    public string Beschrijving { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Categorie { get; set; } = "";

    [Required]
    [Url]
    [StringLength(200)]
    public string GitHubUrl { get; set; } = "";

    [Range(typeof(DateTime), "2000-01-01", "2100-01-01")]
    public DateTime Datum { get; set; }
}
