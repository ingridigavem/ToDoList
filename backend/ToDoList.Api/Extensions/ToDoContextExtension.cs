using Microsoft.Extensions.Caching.Memory;
using ToDoList.Api.Configurations;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Infra.ToDoList.UseCases;

namespace ToDoList.Api.Extensions;

public static class ToDoContextExtension {

    public static void AddToDoContext(this WebApplicationBuilder builder) {
        builder.Services.AddTransient<IToDoRepository, ToDoRepository>();
        builder.Services.AddTransient<MemoryCache>();
    }

    public static void MapToDoEndpoints(this WebApplication app) {
        Endpoints.GetEndpointsV1(app);
    }
}
