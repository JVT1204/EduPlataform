using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class ForumMessage
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
    
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int TopicId { get; set; }
    public int UserId { get; set; }
    
    // Navigation properties
    public virtual ForumTopic Topic { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
