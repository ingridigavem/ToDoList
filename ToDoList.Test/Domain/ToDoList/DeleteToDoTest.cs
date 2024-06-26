using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.DeleteToDo;

namespace ToDoList.Test.Domain.ToDoList;
public class DeleteToDoTest {
    private readonly Mock<IToDoRepository> repository = new();
    private readonly MemoryCache cache = new(new MemoryCacheOptions());
    private const string CACHE_KEY = "ToDoList";

    [Fact]
    public async Task ShouldDeleteToDo() {
        repository.Setup(x => x.DeleteToDoAsync(It.IsAny<Guid>()));
        cache.Set(CACHE_KEY, "test value");

        var handler = new DeleteToDoHandler(repository.Object, cache);

        var request = new DeleteToDoRequest(Guid.NewGuid());
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.NoContent, result.Status);
        Assert.IsType<DeleteToDoResponse>(result.Data);
        Assert.True(result.Data.Success);
        Assert.Empty(result.Errors);
        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.DeleteToDoAsync(It.IsAny<Guid>()));

        var handler = new DeleteToDoHandler(repository.Object, cache);

        var result = await handler.Handle(null);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Fact]
    public async Task ShouldReturnErrorsListMessageGreatherThanZeroAndExeceptionMessageNotNullWhenRepositoryFails() {
        repository.Setup(x => x.DeleteToDoAsync(It.IsAny<Guid>())).Throws(new Exception());

        var handler = new DeleteToDoHandler(repository.Object, cache);

        var request = new DeleteToDoRequest(Guid.NewGuid());
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }
}
