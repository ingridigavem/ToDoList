using Microsoft.EntityFrameworkCore;
using ToDoList.Domain;
using ToDoList.Infra.Data;

namespace ToDoList.Api.Extensions;

public static class BuilderExtension {
    public static void AddConfiguration(this WebApplicationBuilder builder) {
        Configuration.Database.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public static void AddDatabase(this WebApplicationBuilder builder) {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                Configuration.Database.ConnectionString,
                x => x.MigrationsAssembly("ToDoList.Api"))); // store migrations in Api Project
    }

    public static void AddMediator(this WebApplicationBuilder builder) {
        builder.Services.AddMediatR(x
            => x.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
    }
}
