using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDo;
using ToDoList.Test.Shared;

namespace ToDoList.Test.Domain.ToDoList;
public class UpdateToDoDescriptionTest {
    private readonly Mock<IToDoRepository> repository = new();
    private readonly MemoryCache cache = new(new MemoryCacheOptions());

    [Fact]
    public async Task ShouldChangeToDoForDone() {
        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };
        toDoExpected.CompleteToDo();

        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        cache.Set(Consts.CACHE_KEY, "test value");


        var handler = new CompleteToDoHandler(repository.Object, cache);

        var request = new CompleteToDoRequest(toDoExpected.Id);
        var result = await handler.Handle(request, new CancellationToken());

        Assert.True(result.Data.Success);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<CompleteToDoResponse>>(result);
        Assert.IsType<CompleteToDoResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public async Task ShouldReturnResultWithErrorsListMessageGreaterThanZeroAndExceptionMessageNotNullWhenRepositoryFails() {

        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Throws(new Exception()); ;

        var handler = new CompleteToDoHandler(repository.Object, cache);

        var request = new CompleteToDoRequest(new Guid());
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var handler = new CompleteToDoHandler(repository.Object, cache);

        var result = await handler.Handle(null, new CancellationToken());

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }
}
