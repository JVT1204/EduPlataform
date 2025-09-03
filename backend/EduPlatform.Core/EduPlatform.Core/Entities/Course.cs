using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public bool IsPublished { get; set; } = false;
    
    public DateTime? PublishedAt { get; set; }
    
    // Foreign Keys
    public int TeacherId { get; set; }
    
    // Navigation properties
    public virtual User Teacher { get; set; } = null!;
    public virtual ICollection<User> EnrolledStudents { get; set; } = new List<User>();
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}


