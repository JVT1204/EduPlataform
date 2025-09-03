namespace EduPlatform.Core.Entities;

public class UserLessonProgress
{
    public int Id { get; set; }
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTime? CompletedAt { get; set; }
    
    public int ProgressPercentage { get; set; } = 0;
    
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign Keys
    public int UserId { get; set; }
    public int LessonId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
}


