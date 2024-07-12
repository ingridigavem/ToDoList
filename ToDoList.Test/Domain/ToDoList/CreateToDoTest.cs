﻿using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.CreateToDo;
using ToDoList.Test.Shared;
using ToDoList.Test.TestUtils;

namespace ToDoList.Test.Domain.ToDoList;
public class CreateToDoTest {
    private readonly Mock<IToDoRepository> repository = new();
    private readonly MemoryCache cache = new(new MemoryCacheOptions());

    [Fact]
    public async Task ShouldCreateNewToDo() {
        var autoGeneratedValidDescription = Utils.GenerateString(Consts.VALID_LENGTH_DESCRIPTION_EXAMPLE);

        repository.Setup(x => x.SaveToDoAsync(It.IsAny<ToDo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        cache.Set(Consts.CACHE_KEY, "test value");

        var handler = new CreateToDoHandler(repository.Object, cache);

        var request = new CreateToDoRequest(autoGeneratedValidDescription);
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(HttpStatusCode.Created, result.Status);
        Assert.IsType<Result<CreateToDoResponse>>(result);
        Assert.IsType<CreateToDoResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.IsType<Guid>(result.Data.Id);
        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public async Task ShouldNotCreateNewToDoWhenDescriptionMaximumLengthExceeded() {
        var invalidDescriptionLength = Utils.GenerateString(Consts.INVALID_MAXIMUM_LENGTH_DESCRIPTION_EXAMPLE);

        repository.Setup(x => x.SaveToDoAsync(It.IsAny<ToDo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateToDoHandler(repository.Object, cache);

        var request = new CreateToDoRequest(invalidDescriptionLength);
        var result = await handler.Handle(request, new CancellationToken());

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
        repository.Setup(x => x.SaveToDoAsync(It.IsAny<ToDo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateToDoHandler(repository.Object, cache);

        var request = new CreateToDoRequest(description);
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Fact]
    public async Task ShouldReturnResultWithErrorsListMessageGreaterThanZeroAndExceptionMessageNotNullWhenRepositoryFails() {
        var autoGeneratedValidDescription = Utils.GenerateString(Consts.VALID_LENGTH_DESCRIPTION_EXAMPLE);

        repository.Setup(x => x.SaveToDoAsync(It.IsAny<ToDo>(), It.IsAny<CancellationToken>())).Throws(new Exception()); ;

        var handler = new CreateToDoHandler(repository.Object, cache);

        var request = new CreateToDoRequest(autoGeneratedValidDescription);
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.SaveToDoAsync(It.IsAny<ToDo>(), It.IsAny<CancellationToken>()));

        var handler = new CreateToDoHandler(repository.Object, cache);

        var result = await handler.Handle(null, new CancellationToken());

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }
}
