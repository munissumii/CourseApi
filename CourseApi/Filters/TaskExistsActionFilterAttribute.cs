using CourseApi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Filters;

public class TaskExistsActionFilterAttribute : ActionFilterAttribute
{
    private readonly AppDbContext _context;
    public TaskExistsActionFilterAttribute(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if(!context.ActionArguments.ContainsKey("taskId"))
        {
            await next();
            return;
        }

        var taskId = (Guid?)context.ActionArguments["taskId"];

        if(!await _context.Tasks.AnyAsync(u => u.Id == taskId))
        {
            context.Result = new NotFoundResult();
            return;
        }
        await next();
    }
}
