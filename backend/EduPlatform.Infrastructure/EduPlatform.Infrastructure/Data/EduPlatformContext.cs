using Microsoft.EntityFrameworkCore;
using EduPlatform.Core.Entities;

namespace EduPlatform.Infrastructure.Data;

public class EduPlatformContext : DbContext
{
    public EduPlatformContext(DbContextOptions<EduPlatformContext> options) : base(options)
    {
    }

    // DbSets principais
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Alternative> Alternatives { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Progress> Progress { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Forum> Forums { get; set; }
    public DbSet<ForumTopic> ForumTopics { get; set; }
    public DbSet<ForumMessage> ForumMessages { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações das entidades
        ConfigureUser(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureEnrollment(modelBuilder);
        ConfigureModule(modelBuilder);
        ConfigureLesson(modelBuilder);
        ConfigureAssessment(modelBuilder);
        ConfigureQuestion(modelBuilder);
        ConfigureAlternative(modelBuilder);
        ConfigureResponse(modelBuilder);
        ConfigureProgress(modelBuilder);
        ConfigureCertificate(modelBuilder);
        ConfigureForum(modelBuilder);
        ConfigureForumTopic(modelBuilder);
        ConfigureForumMessage(modelBuilder);
        ConfigureNotification(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });
    }

    private void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
    }

    private void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Category)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.TeachingCourses)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureEnrollment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chave única composta para evitar matrículas duplicadas
            entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();
        });
    }

    private void ConfigureModule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Modules)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureLesson(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Module)
                .WithMany(e => e.Lessons)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAssessment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Assessments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Module)
                .WithMany(e => e.Assessments)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureQuestion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Statement).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Weight).HasPrecision(5, 2);
            
            entity.HasOne(e => e.Assessment)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAlternative(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alternative>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
            
            entity.HasOne(e => e.Question)
                .WithMany(e => e.Alternatives)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureResponse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TextResponse).HasMaxLength(2000);
            entity.Property(e => e.AssignedGrade).HasPrecision(5, 2);
            entity.Property(e => e.Feedback).HasMaxLength(1000);
            
            entity.HasOne(e => e.Question)
                .WithMany(e => e.Responses)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(e => e.Responses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Assessment)
                .WithMany()
                .HasForeignKey(e => e.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Alternative)
                .WithMany(e => e.Responses)
                .HasForeignKey(e => e.AlternativeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.GradedBy)
                .WithMany()
                .HasForeignKey(e => e.GradedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureProgress(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Progress)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Lesson)
                .WithMany(e => e.Progress)
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chave única composta para evitar progresso duplicado
            entity.HasIndex(e => new { e.UserId, e.LessonId }).IsUnique();
        });
    }

    private void ConfigureCertificate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VerificationCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.VerificationCode).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Certificates)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureForum(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Forum>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Forums)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureForumTopic(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForumTopic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            
            entity.HasOne(e => e.Forum)
                .WithMany(e => e.Topics)
                .HasForeignKey(e => e.ForumId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(e => e.ForumTopics)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureForumMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForumMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            
            entity.HasOne(e => e.Topic)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(e => e.ForumMessages)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureNotification(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Notifications)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}


