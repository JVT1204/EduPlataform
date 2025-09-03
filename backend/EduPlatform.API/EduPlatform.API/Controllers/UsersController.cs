using Microsoft.AspNetCore.Mvc;
using EduPlatform.Application.DTOs;
using EduPlatform.Core.Interfaces;
using EduPlatform.Core.Entities;

namespace EduPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = users.Select(MapToDto);
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(MapToDto(user));
    }

    [HttpGet("students")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetStudents()
    {
        var students = await _userRepository.GetStudentsAsync();
        var studentDtos = students.Select(MapToDto);
        return Ok(studentDtos);
    }

    [HttpGet("teachers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetTeachers()
    {
        var teachers = await _userRepository.GetTeachersAsync();
        var teacherDtos = teachers.Select(MapToDto);
        return Ok(teacherDtos);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        // TODO: Implementar validação e hash da senha
        var user = new User
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            PasswordHash = createUserDto.Password, // TODO: Hash da senha
            Phone = createUserDto.Phone,
            Role = (UserRole)createUserDto.Role
        };

        var createdUser = await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, MapToDto(createdUser));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        user.Name = updateUserDto.Name;
        user.Phone = updateUserDto.Phone;
        user.IsActive = updateUserDto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userRepository.DeleteAsync(user);
        return NoContent();
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = (UserRoleDto)user.Role,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };
    }
}


