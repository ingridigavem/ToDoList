using ToDoList.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets for Development Environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddConfiguration();
builder.AddDatabase();

builder.AddToDoContext();
builder.AddMediator();

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(
        s => {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoList API v1");
        });
}

app.UseCors();

app.UseHealthChecks("/api/health");
app.MapToDoEndpoints();

app.Run();
