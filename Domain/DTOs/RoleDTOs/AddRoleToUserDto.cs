namespace Domain.DTOs.RoleDTOs;

public class AddRoleToUserDto
{
    public string UserId { get; set; } = null!;
    public string RoleId { get; set; } = null!;
}
