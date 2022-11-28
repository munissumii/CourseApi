using CourseApi.Context;
using CourseApi.Entities;
using CourseApi.Filters;
using CourseApi.Mappers;
using CourseApi.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[TypeFilter(typeof(CourseExistsActionFilterAttribute))]
[TypeFilter(typeof(TaskExistsActionFilterAttribute))]
public partial class CourseController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager; 
    private readonly SignInManager<User> _signInManager;    
    public CourseController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses.ToListAsync();

       List<CourseDto> coursesDto = courses.Select(c => c.ToDto()).ToList();
        return Ok(coursesDto);
    }

    [HttpGet("{courseId}")]
    [IsUserOrAdmin(true)]
    public async Task<IActionResult> GetCourseById(Guid courseId)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

        if (course is null)
            return NotFound();

        return Ok(course?.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CreateCourseDto createCourseDto)
    {
        if(!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.GetUserAsync(User);

        var course = new Course()
        {
            Name = createCourseDto.Name,
            Key = Guid.NewGuid().ToString("N"),
            Users = new List<UserCourse>()
            {
                new UserCourse()
                {
                    UserId = user.Id,
                    IsAdmin = true
                }
            }
        };

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);

        return Ok(course?.ToDto());
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(u => u.Id == courseId);
        if (course is null) return NotFound();


        var user = await _userManager.GetUserAsync(User);
        if (course.Users?.Any(uc => uc.UserId == user.Id && uc.IsAdmin) != true)
            return Forbid();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> UpdateCourse(Guid courseId, [FromBody] UpdateCourseDto updateCourseDto)
    {
        if (!await _context.Courses.AnyAsync(u => u.Id == u.Id))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.GetUserAsync(User);
        var course = await _context.Courses.FirstOrDefaultAsync(u => u.Id == courseId);

        if(course.Users?.Any(uc => uc.UserId == user.Id && uc.IsAdmin) != true) return BadRequest();

        if (course is null) return NotFound();

        course.Name = updateCourseDto.Name;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{courseId}/join")]
    public async Task<IActionResult> JoinCourse(Guid courseId, [FromBody] JoinCourseDto joinCourseDto)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(u => u.Id == courseId);
        if (course is null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (course.Users?.Any(uc => uc.UserId == user.Id ) == true)
            return BadRequest();

        _context.UserCourses.Add(new UserCourse
        {
            UserId = user.Id,
            CourseId = course.Id,
            IsAdmin = false
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

}
