using ToDoList.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddDatabase();

builder.AddToDoContext();
builder.AddMediator();

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(
        s => {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoList API v1");
        });
}

app.UseHealthChecks("/api/health");
app.MapToDoEndpoints();

app.Run();
