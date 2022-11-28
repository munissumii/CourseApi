using CourseApi.Entities;
using CourseApi.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

public partial class CourseController
{
    [HttpGet("{courseId}/tasks/{taskId}/comments")]
    public async Task<IActionResult> GetTaskComments(Guid courseId, Guid taskId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(u => u.Id == taskId && u.CourseId == courseId);
        if (task is null) return NotFound();

        var comments = new List<TaskCommentDto>();
        if (task.Comments is null) return Ok(comments);

        var mainComments = task.Comments.Where(c => c.ParentId == null).ToList();
        foreach(var comment in mainComments)
        {
            var commentDto = ToTaskCommentDto(comment);
            comments.Add(commentDto);
        }

        return Ok(comments);
    }

    private TaskCommentDto ToTaskCommentDto(TaskComment comment)
    {
        var commentDto = new TaskCommentDto()
        {
            Id = comment.Id,
            Comment = comment.Comment,
            CreatedDate = comment.CreatedDate,
            User = comment.User?.Adapt<UserDto>()
        };

        if (comment.Children is null)
            return commentDto;

        foreach(var child in comment.Children)
        {
            commentDto.Children ??= new List<TaskCommentDto>();
            commentDto.Children.Add(ToTaskCommentDto(child));
        }

        return commentDto;
    }

    [HttpPost("{courseId}/tasks/{taskId}/comments")]
    public async Task<IActionResult> AddTAskComments(Guid courseId, Guid taskId, CreateTaskCommentDto commentDto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(u => u.Id == taskId && u.CourseId == courseId);
        if (task is null) return NotFound();

        var user = await _userManager.GetUserAsync(User);

        task.Comments ??= new List<TaskComment>();

        task.Comments.Add(new TaskComment()
        {
                TaskId = taskId,
                UserId = user.Id,
                Comment = commentDto.Comment,
                ParentId = commentDto.ParentId,
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

}
