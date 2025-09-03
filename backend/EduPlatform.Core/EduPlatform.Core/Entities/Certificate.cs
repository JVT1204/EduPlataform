using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Certificate
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string VerificationCode { get; set; } = string.Empty;
    
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsValid { get; set; } = true;
    
    // Foreign Keys
    public int UserId { get; set; }
    public int CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}
