using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Assignment
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public int MaxScore { get; set; } = 100;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int CourseId { get; set; }
    public int CreatedById { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

public class AssignmentSubmission
{
    public int Id { get; set; }
    
    [StringLength(1000)]
    public string? Content { get; set; }
    
    [StringLength(500)]
    public string? FileUrl { get; set; }
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    public int? Score { get; set; }
    
    public string? Feedback { get; set; }
    
    public DateTime? GradedAt { get; set; }
    
    public int? GradedById { get; set; }
    
    // Foreign Keys
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    
    // Navigation properties
    public virtual Assignment Assignment { get; set; } = null!;
    public virtual User Student { get; set; } = null!;
    public virtual User? GradedBy { get; set; }
}


