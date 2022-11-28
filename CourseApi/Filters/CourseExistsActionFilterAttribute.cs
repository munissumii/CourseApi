using CourseApi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Filters;

public class CourseExistsActionFilterAttribute : ActionFilterAttribute
{
    private readonly AppDbContext _context;
    public CourseExistsActionFilterAttribute(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.ContainsKey("courseId"))
        {
            await next();
            return;
        }

        var courseId = (Guid?)context.ActionArguments["courseId"];
        if(!await  _context.Courses.AnyAsync(u=>u.Id == courseId))
        {
            context.Result = new NotFoundResult();
            return;
        }

        await next();
    }

}
