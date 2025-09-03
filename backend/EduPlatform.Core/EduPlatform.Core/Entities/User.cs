using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? Phone { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public UserRole Role { get; set; } = UserRole.Student;
    
    // Navigation properties
    public virtual ICollection<Course> EnrolledCourses { get; set; } = new List<Course>();
    public virtual ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
    public virtual ICollection<AssignmentSubmission> SubmittedAssignments { get; set; } = new List<AssignmentSubmission>();
}

public enum UserRole
{
    Student,
    Teacher,
    Admin
}
