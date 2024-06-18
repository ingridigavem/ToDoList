using ToDoList.Domain.ToDoList.UseCases.CreateToDo.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICreateToDoRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
