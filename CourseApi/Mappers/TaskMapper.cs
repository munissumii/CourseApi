using CourseApi.Models;

namespace CourseApi.Mappers
{
    public static class TaskMapper
    {
        public static void SetValues(this CourseApi.Entities.Task task, UpdateTaskResultDto updateTaskDto)
        {
            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Status = updateTaskDto.Status;
            task.StartDate = updateTaskDto.StartDate;
            task.EndDate = updateTaskDto.EndDate;
            task.MaxScore = updateTaskDto.MaxScore;
        }
    }
}
