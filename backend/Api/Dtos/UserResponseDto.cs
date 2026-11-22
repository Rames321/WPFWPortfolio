namespace Api.Dtos;

public class UserResponseDto
{
    public string Email { get; set; } = "";
    public List<string> Rollen { get; set; } = new();
}
