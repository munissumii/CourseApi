using CourseApi.Context;
using CourseApi.Entities;
using CourseApi.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDataAnnotationsLocalization(option =>
{

    option.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Program));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resourses";
});
builder.Services.AddLocalization(option =>
{
    //  option.
});

var logger = new LoggerConfiguration()
   .WriteTo.Console()
   .WriteTo.File(path: "classroom.log");

//builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlite("Data source=Db.db");
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireUppercase = false;

}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<LocalizerService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new RequestCulture(new CultureInfo("uz"));
    options.SupportedUICultures = new List<CultureInfo>()
    {
        new CultureInfo("uz"),
        new CultureInfo("ru"),
        new CultureInfo("en")
    };

    options.SupportedCultures = new List<CultureInfo>()
    {
        new CultureInfo("uz"),
        new CultureInfo("ru"),
        new CultureInfo("en")
    };
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
