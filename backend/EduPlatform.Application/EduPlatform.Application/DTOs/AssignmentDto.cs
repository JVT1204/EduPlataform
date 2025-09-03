namespace EduPlatform.Application.DTOs;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public int SubmissionsCount { get; set; }
}

public class CreateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxScore { get; set; } = 100;
}

public class UpdateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxScore { get; set; }
    public bool IsActive { get; set; }
}



