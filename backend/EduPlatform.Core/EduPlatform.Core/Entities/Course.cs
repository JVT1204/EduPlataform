using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public CourseStatus Status { get; set; } = CourseStatus.Draft;
    
    // Foreign Keys
    public int CategoryId { get; set; }
    public int TeacherId { get; set; }
    
    // Navigation properties
    public virtual Category Category { get; set; } = null!;
    public virtual User Teacher { get; set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    public virtual ICollection<Forum> Forums { get; set; } = new List<Forum>();
}

public enum CourseStatus
{
    Draft,
    Active,
    Archived
}


