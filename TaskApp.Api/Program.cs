using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Tasks.Commands.CreateTask;
using TaskApp.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using TaskApp.Application.Common.Behaviors;
using TaskApp.Infrastructure.Caching;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<TaskAppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "TaskApp API",
        Version = "v1",
        Description = "Modular monolith task management backend"
    });

    // Future-proof for JWT support
    /*
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
    */
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskApp API V1");
    options.RoutePrefix = string.Empty; // Makes Swagger the root page
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();