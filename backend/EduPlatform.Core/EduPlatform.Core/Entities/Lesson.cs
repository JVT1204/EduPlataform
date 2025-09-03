using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Lesson
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public int Order { get; set; }
    
    public LessonType Type { get; set; } = LessonType.Video;
    
    [StringLength(500)]
    public string? FileUrl { get; set; }
    
    public string? Content { get; set; }
    
    public int DurationMinutes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int ModuleId { get; set; }
    
    // Navigation properties
    public virtual Module Module { get; set; } = null!;
    public virtual ICollection<Progress> Progress { get; set; } = new List<Progress>();
}

public enum LessonType
{
    Video,
    PDF,
    Quiz,
    Link,
    PracticalActivity,
    Text
}


