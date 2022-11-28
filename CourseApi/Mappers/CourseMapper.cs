using CourseApi.Entities;
using CourseApi.Models;
using Mapster;

namespace CourseApi.Mappers;

public static class CourseMapper
{
    public static CourseDto ToDto(this Course course) 
    {
        return new CourseDto
        { 
        Id = course.Id,
        Name = course.Name,
        Key = course.Key,
        Users = course.Users?.Select(usercourse => usercourse.User?.Adapt<UserDto>()).ToList()

        };
    }
}
