using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Response
{
    public int Id { get; set; }
    
    [StringLength(2000)]
    public string? TextResponse { get; set; }
    
    public decimal? AssignedGrade { get; set; }
    
    public string? Feedback { get; set; }
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? GradedAt { get; set; }
    
    public int? GradedById { get; set; }
    
    // Foreign Keys
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int AssessmentId { get; set; }
    public int? AlternativeId { get; set; }
    
    // Navigation properties
    public virtual Question Question { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Assessment Assessment { get; set; } = null!;
    public virtual Alternative? Alternative { get; set; }
    public virtual User? GradedBy { get; set; }
}
