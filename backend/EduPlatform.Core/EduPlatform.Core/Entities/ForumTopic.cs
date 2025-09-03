using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class ForumTopic
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
    
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public bool IsPinned { get; set; } = false;
    
    public bool IsLocked { get; set; } = false;
    
    // Foreign Keys
    public int ForumId { get; set; }
    public int UserId { get; set; }
    
    // Navigation properties
    public virtual Forum Forum { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<ForumMessage> Messages { get; set; } = new List<ForumMessage>();
}
