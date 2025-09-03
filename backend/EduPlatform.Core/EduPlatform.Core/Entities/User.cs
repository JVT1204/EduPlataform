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
    
    public DateTime? LastLogin { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public UserRole Role { get; set; } = UserRole.Student;
    
    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
    public virtual ICollection<Progress> Progress { get; set; } = new List<Progress>();
    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public virtual ICollection<ForumTopic> ForumTopics { get; set; } = new List<ForumTopic>();
    public virtual ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

public enum UserRole
{
    Student,
    Teacher,
    Admin
}
