using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Notification
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
    
    public bool IsRead { get; set; } = false;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ReadAt { get; set; }
    
    public NotificationType Type { get; set; } = NotificationType.General;
    
    // Foreign Keys
    public int UserId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}

public enum NotificationType
{
    General,
    Course,
    Assessment,
    Forum,
    System
}
