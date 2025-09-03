using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Core.Entities;

public class Question
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Statement { get; set; } = string.Empty;
    
    public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
    
    public decimal Weight { get; set; } = 1.0m;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public int AssessmentId { get; set; }
    
    // Navigation properties
    public virtual Assessment Assessment { get; set; } = null!;
    public virtual ICollection<Alternative> Alternatives { get; set; } = new List<Alternative>();
    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}

public enum QuestionType
{
    MultipleChoice,
    Essay,
    TrueFalse
}


