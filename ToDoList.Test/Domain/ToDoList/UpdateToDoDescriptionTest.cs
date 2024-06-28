using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.UpdateToDoDescription;
using ToDoList.Test.Shared;
using ToDoList.Test.TestUtils;

namespace ToDoList.Test.Domain.ToDoList;
public class CompleteToDoTest {
    private readonly Mock<IToDoRepository> repository = new();
    private readonly MemoryCache cache = new(new MemoryCacheOptions());

    [Fact]
    public async Task ShouldUpdateToDoDescription() {
        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };

        toDoExpected.CompleteToDo();

        repository.Setup(x => x.UpdateToDoDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(toDoExpected);
        cache.Set(Consts.CACHE_KEY, "test value");

        var handler = new UpdateToDoDescriptionHandler(repository.Object, cache);

        var request = new UpdateToDoDescriptionRequest(toDoExpected.Id, Consts.DESCRIPTION_FOR_TEST);
        var result = await handler.Handle(request);

        Assert.Equal(toDoExpected, result.Data.ToDo);
        Assert.Equal(toDoExpected.Description, result.Data.ToDo.Description);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<UpdateToDoDescriptionResponse>>(result);
        Assert.IsType<UpdateToDoDescriptionResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public async Task ShouldNotUpdateToDoWhenDescriptionMaximumLengthExceeded() {
        var invalidDescriptionLength = Utils.GenerateString(Consts.INVALID_MAXIMUM_LENGTH_DESCRIPTION_EXAMPLE);

        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };

        repository.Setup(x => x.UpdateToDoDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(toDoExpected);

        var handler = new UpdateToDoDescriptionHandler(repository.Object, cache);

        var request = new UpdateToDoDescriptionRequest(toDoExpected.Id, invalidDescriptionLength);
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("X")]
    [InlineData("XX")]
    [InlineData(null)]
    public async Task ShouldNotCreateNewToDoWhenDescriptionLengthLowerThanMinimumOrEmptyOrNull(string description) {
        var toDoExpected = new ToDo { Description = Consts.DESCRIPTION_FOR_TEST };

        repository.Setup(x => x.UpdateToDoDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(toDoExpected);

        var handler = new UpdateToDoDescriptionHandler(repository.Object, cache);

        var request = new UpdateToDoDescriptionRequest(toDoExpected.Id, description);
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Fact]
    public async Task ShouldReturnResultWithErrorsListMessageGreaterThanZeroAndExceptionMessageNotNullWhenRepositoryFails() {
        repository.Setup(x => x.UpdateToDoDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new Exception()); ;

        var handler = new UpdateToDoDescriptionHandler(repository.Object, cache);

        var request = new UpdateToDoDescriptionRequest(new Guid(), Consts.DESCRIPTION_FOR_TEST);
        var result = await handler.Handle(request);

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.UpdateToDoDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>()));

        var handler = new UpdateToDoDescriptionHandler(repository.Object, cache);

        var result = await handler.Handle(null);

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }
}
