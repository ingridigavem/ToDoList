using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.DeleteToDo;

namespace ToDoList.Test.Domain.ToDoList;
public class DeleteToDoTest {
    Mock<IToDoRepository> repository = new Mock<IToDoRepository>();
    Mock<IMemoryCache> cache = new Mock<IMemoryCache>();
    private const string CACHE_KEY = "ToDoList";

    [Fact]
    public async Task ShouldDeleteToDo() {
        repository.Setup(x => x.DeleteToDo(It.IsAny<Guid>()));
        cache.Setup(x => x.CreateEntry(CACHE_KEY));

        var handler = new DeleteToDoHandler(repository.Object, cache.Object);

        var request = new DeleteToDoRequest(Guid.NewGuid());
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<DeleteToDoResponse>(result.Data);
        Assert.True(result.Data.Success);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.DeleteToDo(It.IsAny<Guid>()));
        cache.Setup(x => x.CreateEntry(CACHE_KEY));

        var handler = new DeleteToDoHandler(repository.Object, cache.Object);

        var result = await handler.Handle(null);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Fact]
    public async Task ShouldReturnErrorsListMessageGreatherThanZeroAndExeceptionMessageNotNullWhenRepositoryFails() {
        repository.Setup(x => x.DeleteToDo(It.IsAny<Guid>())).Throws(new Exception());
        cache.Setup(x => x.CreateEntry(CACHE_KEY));

        var handler = new DeleteToDoHandler(repository.Object, cache.Object);

        var request = new DeleteToDoRequest(Guid.NewGuid());
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }
}
