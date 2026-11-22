using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class BlogpostUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Titel { get; set; } = "";

    [Required]
    [StringLength(10000)]
    public string Inhoud { get; set; } = "";

    [Range(typeof(DateTime), "2000-01-01", "2100-01-01")]
    public DateTime Publicatiedatum { get; set; }
}
