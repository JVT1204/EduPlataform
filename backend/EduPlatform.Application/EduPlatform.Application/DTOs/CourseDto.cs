namespace EduPlatform.Application.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public UserDto Teacher { get; set; } = null!;
    public int EnrolledStudentsCount { get; set; }
    public int ModulesCount { get; set; }
}

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
}

public class CourseDetailDto : CourseDto
{
    public List<ModuleDto> Modules { get; set; } = new();
    public List<AssignmentDto> Assignments { get; set; } = new();
    public List<UserDto> EnrolledStudents { get; set; } = new();
}


