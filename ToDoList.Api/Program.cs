using ToDoList.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddDatabase();

builder.AddToDoContext();
builder.AddMediator();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapToDoEndpoints();

app.Run();
