using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Assessment
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public AssessmentType Type { get; set; } = AssessmentType.Quiz;
    
    public DateTime AvailableDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int CourseId { get; set; }
    public int? ModuleId { get; set; }
    public int CreatedById { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual Module? Module { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}

public enum AssessmentType
{
    Quiz,
    Exam,
    Assignment
}


