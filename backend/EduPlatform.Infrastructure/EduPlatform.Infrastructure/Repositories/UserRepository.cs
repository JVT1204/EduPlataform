using Microsoft.EntityFrameworkCore;
using EduPlatform.Core.Entities;
using EduPlatform.Core.Interfaces;
using EduPlatform.Infrastructure.Data;

namespace EduPlatform.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(EduPlatformContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.EnrolledCourses)
            .Include(u => u.TeachingCourses)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetStudentsAsync()
    {
        return await _dbSet
            .Where(u => u.Role == UserRole.Student && u.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetTeachersAsync()
    {
        return await _dbSet
            .Where(u => u.Role == UserRole.Teacher && u.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetEnrolledCoursesAsync(int userId)
    {
        var user = await _dbSet
            .Include(u => u.EnrolledCourses)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        return user?.EnrolledCourses ?? new List<Course>();
    }

    public async Task<IEnumerable<Course>> GetTeachingCoursesAsync(int userId)
    {
        var user = await _dbSet
            .Include(u => u.TeachingCourses)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        return user?.TeachingCourses ?? new List<Course>();
    }
}


