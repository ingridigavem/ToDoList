using ToDoList.Domain.ToDoList.UseCases.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddTransient<IToDoRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
