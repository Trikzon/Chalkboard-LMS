using Lms.BackEnd.WebApi.Services;

namespace Lms.BackEnd.WebApi;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddSingleton<ContentItemService>();
        builder.Services.AddSingleton<CourseService>();
        builder.Services.AddSingleton<EnrollmentService>();
        builder.Services.AddSingleton<ModuleService>();
        builder.Services.AddSingleton<StudentService>();
        builder.Services.AddSingleton<SubmissionService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}