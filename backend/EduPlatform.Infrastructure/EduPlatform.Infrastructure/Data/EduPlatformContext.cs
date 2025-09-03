using Microsoft.EntityFrameworkCore;
using EduPlatform.Core.Entities;

namespace EduPlatform.Infrastructure.Data;

public class EduPlatformContext : DbContext
{
    public EduPlatformContext(DbContextOptions<EduPlatformContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
    public DbSet<UserLessonProgress> UserLessonProgress { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações das entidades
        ConfigureUser(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureModule(modelBuilder);
        ConfigureLesson(modelBuilder);
        ConfigureAssignment(modelBuilder);
        ConfigureAssignmentSubmission(modelBuilder);
        ConfigureUserLessonProgress(modelBuilder);
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

    private void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.TeachingCourses)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.EnrolledStudents)
                .WithMany(e => e.EnrolledCourses)
                .UsingEntity(j => j.ToTable("CourseEnrollments"));
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
            entity.Property(e => e.ContentUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Module)
                .WithMany(e => e.Lessons)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureAssignmentSubmission(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.Feedback).HasMaxLength(1000);
            
            entity.HasOne(e => e.Assignment)
                .WithMany(e => e.Submissions)
                .HasForeignKey(e => e.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Student)
                .WithMany(e => e.SubmittedAssignments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.GradedBy)
                .WithMany()
                .HasForeignKey(e => e.GradedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureUserLessonProgress(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLessonProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Lesson)
                .WithMany(e => e.UserProgress)
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chave única composta para evitar progresso duplicado
            entity.HasIndex(e => new { e.UserId, e.LessonId }).IsUnique();
        });
    }
}


