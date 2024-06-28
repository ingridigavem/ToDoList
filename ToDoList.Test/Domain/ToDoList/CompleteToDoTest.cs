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
    private const bool DONE = true;
    private const bool UNDONE = false;

    [Fact]
    public async Task ShouldChangeToDoForDone() {
        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };
        toDoExpected.CompleteToDo();

        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(toDoExpected);
        cache.Set(Consts.CACHE_KEY, "test value");


        var handler = new CompleteToDoHandler(repository.Object, cache);

        var request = new CompleteToDoRequest(toDoExpected.Id, DONE);
        var result = await handler.Handle(request);

        Assert.Equal(toDoExpected, result.Data.ToDo);
        Assert.True(result.Data.ToDo.Done);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<CompleteToDoResponse>>(result);
        Assert.IsType<CompleteToDoResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public async Task ShouldChangeToDoForUndone() {
        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };
        toDoExpected.CompleteToDo(); // change to TRUE
        toDoExpected.CompleteToDo(); // change to FALSE

        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(toDoExpected);
        cache.Set(Consts.CACHE_KEY, "test value");

        var handler = new CompleteToDoHandler(repository.Object, cache);

        var request = new CompleteToDoRequest(toDoExpected.Id, UNDONE);
        var result = await handler.Handle(request);

        Assert.Equal(toDoExpected, result.Data.ToDo);
        Assert.False(result.Data.ToDo.Done);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<CompleteToDoResponse>>(result);
        Assert.IsType<CompleteToDoResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(0, cache.Count);
    }

    [Theory]
    [InlineData(DONE)]
    [InlineData(UNDONE)]
    public async Task ShouldReturnResultWithErrorsListMessageGreaterThanZeroAndExceptionMessageNotNullWhenRepositoryFails(bool complete) {

        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Throws(new Exception()); ;

        var handler = new CompleteToDoHandler(repository.Object, cache);

        var request = new CompleteToDoRequest(new Guid(), complete);
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.CompleteToDoAsync(It.IsAny<Guid>(), It.IsAny<bool>()));

        var handler = new CompleteToDoHandler(repository.Object, cache);

        var result = await handler.Handle(null);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }
}
