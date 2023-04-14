using Microsoft.EntityFrameworkCore;
using SchoolMinimalApi.DTOs;
using SchoolMinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolMinimalApiConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.MapGet("/", () => "Hello World!");

var minimalApi = app.MapGroup("courses");

minimalApi.MapGet("/", async (SchoolDbContext dbContext) =>
{
    return Results.Ok(await dbContext.Courses.Select(c => new CourseDto(c)).ToListAsync());
});

minimalApi.MapGet("/{id}", async (int id, SchoolDbContext dbContext) =>
{
    var course = await dbContext.Courses.FindAsync(id);

    if (course == null) return Results.NotFound("Ce cours n'existe pas");

    var courseDto = new CourseDto(course);

    return Results.Ok(courseDto);
});

minimalApi.MapPost("/", async (CourseDto courseDto, SchoolDbContext dbContext) =>
{
    var course = new Course
    {
        Name = courseDto.Name
    };
    dbContext.Courses.Add(course);
    await dbContext.SaveChangesAsync();
    var courseCreateDto = new CourseDto(course);
    return Results.Created($"/course/{courseCreateDto.Id}", courseCreateDto);
});

minimalApi.MapPut("/{id}", async (int id, CourseDto courseDto, SchoolDbContext dbContext) =>
{
    var course = dbContext.Courses.Find(id);
    if (course == null) return Results.NotFound("Cours inéxistant");

    course.Name = courseDto.Name;
    await dbContext.SaveChangesAsync();
    return Results.NoContent(); 
});

minimalApi.MapDelete("/{id}", async (int id, SchoolDbContext dbContext) =>
{
    var course = dbContext.Courses.Find(id);
    if(course == null) return Results.NotFound("Nous n'avons pas ce cours");
    dbContext.Courses.Remove(course);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
