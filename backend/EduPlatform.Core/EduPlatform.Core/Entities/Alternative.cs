using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Alternative
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Text { get; set; } = string.Empty;
    
    public bool IsCorrect { get; set; } = false;
    
    public int Order { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int QuestionId { get; set; }
    
    // Navigation properties
    public virtual Question Question { get; set; } = null!;
    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}


