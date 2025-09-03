namespace EduPlatform.Application.DTOs;

public class ModuleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public int LessonsCount { get; set; }
}

public class CreateModuleDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}

public class UpdateModuleDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}


