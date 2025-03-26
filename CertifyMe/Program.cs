using CertifyMe.Middlewares;
using CertifyMe.Models.Database;
using CertifyMe.Repositories;
using CertifyMe.Services;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load(".env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectiion"))
    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONN"))
);

builder.Services.AddSingleton<ITaskQueueService, TaskQueueService>();
builder.Services.AddSingleton<IImportExcelService, ImportExcelService>();
builder.Services.AddTransient<ICourseCompletionRepository, CourseCompletionRepository>();

builder.Services.AddHostedService<TaskQueueWorker>();
builder.Services.AddHostedService<CertificateGenWorker>();
//builder.Services.AddHostedService<CertificateSendWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }
        await next();
    });
}

app.UseMiddleware(typeof(ErrorHandlingMiddleware));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
