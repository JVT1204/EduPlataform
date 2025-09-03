using Microsoft.EntityFrameworkCore;
using EduPlatform.Core.Entities;
using EduPlatform.Core.Interfaces;
using EduPlatform.Infrastructure.Data;

namespace EduPlatform.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(EduPlatformContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
    {
        return await _dbSet
            .Include(c => c.Teacher)
            .Include(c => c.Category)
            .Include(c => c.Modules)
            .Where(c => c.Status == CourseStatus.Active && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId)
    {
        return await _dbSet
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.User)
            .Include(c => c.Modules)
            .Where(c => c.TeacherId == teacherId && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetEnrolledStudentsAsync(int courseId)
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.User)
            .Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Active)
            .ToListAsync();
        
        return enrollments.Select(e => e.User);
    }

    public async Task<bool> IsStudentEnrolledAsync(int courseId, int studentId)
    {
        return await _context.Enrollments
            .AnyAsync(e => e.CourseId == courseId && e.UserId == studentId && e.Status == EnrollmentStatus.Active);
    }
}


