using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Forum
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int CourseId { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual ICollection<ForumTopic> Topics { get; set; } = new List<ForumTopic>();
}
