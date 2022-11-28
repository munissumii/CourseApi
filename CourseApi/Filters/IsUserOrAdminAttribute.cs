using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Filters;

public class IsUserOrAdminAttribute : TypeFilterAttribute
{
    public IsUserOrAdminAttribute(bool onlyAdmin = false) : base(typeof(CourseAdminFilterAttribute))
    {
        Arguments = new object[] { onlyAdmin };
    }
}
