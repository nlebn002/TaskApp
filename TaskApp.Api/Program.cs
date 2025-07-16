using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Tasks.Commands.CreateTask;
using TaskApp.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using TaskApp.Application.Common.Behaviors;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<TaskAppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add controller services

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>());

builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();