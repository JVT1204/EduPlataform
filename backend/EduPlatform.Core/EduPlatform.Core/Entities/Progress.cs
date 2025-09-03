using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Progress
{
    public int Id { get; set; }
    
    public ProgressStatus Status { get; set; } = ProgressStatus.NotStarted;
    
    public int Percentage { get; set; } = 0;
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedAt { get; set; }
    
    // Foreign Keys
    public int UserId { get; set; }
    public int LessonId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
}

public enum ProgressStatus
{
    NotStarted,
    InProgress,
    Completed
}
