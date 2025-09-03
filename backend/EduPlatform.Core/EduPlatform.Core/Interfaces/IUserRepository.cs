using EduPlatform.Core.Entities;

namespace EduPlatform.Core.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetStudentsAsync();
    Task<IEnumerable<User>> GetTeachersAsync();
    Task<IEnumerable<Course>> GetEnrolledCoursesAsync(int userId);
    Task<IEnumerable<Course>> GetTeachingCoursesAsync(int userId);
}


