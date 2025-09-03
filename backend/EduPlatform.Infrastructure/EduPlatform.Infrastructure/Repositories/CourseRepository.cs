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
            .Include(c => c.Modules)
            .Where(c => c.IsPublished && c.IsActive)
            .OrderByDescending(c => c.PublishedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId)
    {
        return await _dbSet
            .Include(c => c.EnrolledStudents)
            .Include(c => c.Modules)
            .Where(c => c.TeacherId == teacherId && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetEnrolledStudentsAsync(int courseId)
    {
        var course = await _dbSet
            .Include(c => c.EnrolledStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        
        return course?.EnrolledStudents ?? new List<User>();
    }

    public async Task<bool> IsStudentEnrolledAsync(int courseId, int studentId)
    {
        var course = await _dbSet
            .Include(c => c.EnrolledStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        
        return course?.EnrolledStudents.Any(s => s.Id == studentId) ?? false;
    }
}


