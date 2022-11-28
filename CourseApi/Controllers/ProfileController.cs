using CourseApi.Context;
using CourseApi.Entities;
using CourseApi.Mappers;
using CourseApi.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserTask = CourseApi.Entities.UserTask;

namespace CourseApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProfileController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public ProfileController(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("courses")]
    [ProducesResponseType(typeof(List<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        var coursesDto = user.Courses?.Select(userCourse => userCourse.Course.ToDto()).ToList();
        return Ok(coursesDto);
    }

    [HttpGet("courses/{courseId}/tasks")]
    [ProducesResponseType(typeof(List<UserTaskResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTasks(Guid courseId)
    {
        var user = await _userManager.GetUserAsync(User);

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course?.Tasks is null)    return NotFound();

        var tasks = course.Tasks;
        var usertasks = new List<UserTaskResultDto>();

        foreach (var task in tasks)
        {
            var result = task.Adapt<UsersTaskResultsDto>();
            var userResultEntity = task.UserTasks?.FirstOrDefault(ut => ut.UserId == user.Id);
            /*
            result.UserResult = userResultEntity == null ? null : new UserTaskResult()
            {
                Status = userResultEntity.Status,
                Description = userResultEntity.Description
            };

            usertasks.Add(result);
            */
        }

        return Ok(usertasks);
    }

    [HttpPost("courses/{courseId}/tasks/{taskId}")]
    public async Task<IActionResult> AddUserTaskResult(Guid courseId, Guid taskId, [FromBody] CreateUserTaskResultDto resultDto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
        if (task is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var userTaskResult = await _context.UserTasks
            .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TaskId == taskId);

        if (userTaskResult is null)
        {
            userTaskResult = new UserTask()
            {
                UserId = user.Id,
                TaskId = taskId
            };

            await _context.UserTasks.AddAsync(userTaskResult);
        }

        userTaskResult.Description = resultDto.Description;
        userTaskResult.Status = resultDto.Status;

        await _context.SaveChangesAsync();

        return Ok();
    }

}

/*
#Note

1 - 
2 - Tasklani ichidan userIdga courseId true buganini olib
3 - taskni status va descriptioni pars qilinadi
4 - UserTaskResultga Id usernameni add qilamiz
5 - ResultIdga qarab Taskni update qilamiz
6 - 


#MAp

1 - Entitylarni Dtoga Map qilish
2 - 
return ok(Adapt try!!!!)dto


#Logger

1 - Serilog.Aspnetcore  / Serilog.Sinks.File/Console
loggerConfigurationdan object olamiz .WriteTo.Console/File(path,
                                                logLevel(error, fatal, info), 
                                                RollingInterval(file settinglari 
    (.Day[har kun yangi fayl ochadi])           OutputTemplete();
                                     .CreateLogger();

2 - programda builder.Logging.AddSerilog(
*/




/*
        
      typefilter = actionfilterni dependsilarini olib beradi???????????????????
      async attribute filter funksiyalariga keyingisini chaqirib berish shart, void busa wartmas await next()
      

*/



/*
    Add Cors to buider service
    Add policy to option in cors, ("string", corsPolicyBuilder) [allow header, method, origin, creidentials(cookiega access beradi)]
               boshqa clientdan request berganda bloklamaslik uchun
    UseCors();
    UseMiddleware();
    
  * LocalizedStringEntity => Do DbSet table
    HasData uchun OnModelCreating(protected override) base.ModelCreatinggaga !!! builderni berish kk
   
    LocalizerService=> dbdan culturga(CultureInfo.CurrentCulture.Name) qarab languageni qaytarib berishimiz kk, key bn;
    localizedstring null bulsa keyni return qiladi
    Serviceni addScoped qilish kk
    
  * IMemorycashe ,  culturga(CultureInfo.CurrentCulture.Name) qarab languageni aniqlaymiz, key
    
    
    ********* TABLE CONFIGURATION *********

    [Table(courses)] = nameni change qilish uchun
    [StringLength(100, MinimumLength = 10)]
        
        












*/