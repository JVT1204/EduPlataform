using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Enrollment
{
    public int Id { get; set; }
    
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    
    public DateTime? CompletedAt { get; set; }
    
    // Foreign Keys
    public int UserId { get; set; }
    public int CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}

public enum EnrollmentStatus
{
    Active,
    Locked,
    Completed
}


