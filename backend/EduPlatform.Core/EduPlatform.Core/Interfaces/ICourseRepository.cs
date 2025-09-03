using EduPlatform.Core.Entities;

namespace EduPlatform.Core.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetPublishedCoursesAsync();
    Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId);
    Task<IEnumerable<User>> GetEnrolledStudentsAsync(int courseId);
    Task<bool> IsStudentEnrolledAsync(int courseId, int studentId);
}


