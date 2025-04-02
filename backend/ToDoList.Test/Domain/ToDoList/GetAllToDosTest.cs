using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;
using ToDoList.Domain.Shared.DTOs;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Domain.ToDoList.UseCases.Contracts;
using ToDoList.Domain.ToDoList.UseCases.GetAllToDos;

namespace ToDoList.Test.Domain.ToDoList;
public class GetAllToDosTest {
    private readonly Mock<IToDoRepository> repository = new();
    private readonly MemoryCache cache = new(new MemoryCacheOptions());

    [Fact]
    public async Task ShouldReturnListOfToDosWithDefaultPagination() {
        var numberOfToDosWithDefaultPagination = 10;
        var totalCacheExpected = 2;
        var toDosListExpected = GetToDosMock(numberOfToDosWithDefaultPagination);

        repository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(toDosListExpected);
        repository.Setup(x => x.CountAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(toDosListExpected.Count);

        var handler = new GetAllToDosHandler(repository.Object, cache);

        var request = new GetAllToDosRequest();
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(toDosListExpected, result.Data.ToDos);
        Assert.Equal(toDosListExpected.Count, result.Data.TotalCount);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<GetAllToDosResponse>>(result);
        Assert.IsType<GetAllToDosResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(totalCacheExpected, cache.Count);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 100)]
    [InlineData(3, 20)]
    public async Task ShouldReturnListOfToDosUsingCache(int pageNumber, int pageSize) {
        var totalCacheExpected = 2;
        var toDosListExpected = GetToDosMock(pageSize);

        repository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
        repository.Setup(x => x.CountAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        cache.Set($"toDosList_page_{pageNumber}_size_{pageSize}_includeDeleted_{false}", toDosListExpected);
        cache.Set("totalCount", pageSize);

        var handler = new GetAllToDosHandler(repository.Object, cache);

        var request = new GetAllToDosRequest(pageNumber, pageSize);
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(toDosListExpected, result.Data.ToDos);
        Assert.Equal(toDosListExpected.Count, result.Data.TotalCount);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<GetAllToDosResponse>>(result);
        Assert.IsType<GetAllToDosResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(totalCacheExpected, cache.Count);
    }

    [Fact]
    public async Task ShouldReturnEmptyListOfToDos() {
        var emptyToDoList = new List<ToDo>();
        var totalCacheExpected = 2;
        var totalCountExpected = 0;

        repository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(emptyToDoList);
        repository.Setup(x => x.CountAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(totalCountExpected);

        var handler = new GetAllToDosHandler(repository.Object, cache);

        var request = new GetAllToDosRequest();
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Empty(result.Data.ToDos);
        Assert.Equal(totalCountExpected, result.Data.TotalCount);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.IsType<Result<GetAllToDosResponse>>(result);
        Assert.IsType<GetAllToDosResponse>(result.Data);
        Assert.Empty(result.Errors);
        Assert.Equal(totalCacheExpected, cache.Count);
    }

    [Fact]
    public async Task ShouldReturnErrorWithInvalidRequest() {
        repository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        var handler = new GetAllToDosHandler(repository.Object, cache);

        var result = await handler.Handle(null, new CancellationToken());

        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Data);
        Assert.Null(result.ExceptionMessage);
    }

    [Fact]
    public async Task ShouldReturnErrorsListMessageGreatherThanZeroAndExeceptionMessageNotNullWhenRepositoryFails() {
        repository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Throws(new Exception());

        var handler = new GetAllToDosHandler(repository.Object, cache);

        var request = new GetAllToDosRequest();
        var result = await handler.Handle(request, new CancellationToken());

        Assert.Equal(HttpStatusCode.InternalServerError, result.Status);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.ExceptionMessage);
        Assert.Null(result.Data);
    }

    private static List<ToDo> GetToDosMock(int numberOfToDos) {
        var toDoListMock = new List<ToDo>();
        for (int i = 0; i < numberOfToDos; i++) {
            toDoListMock.Add(new ToDo {
                Description = $"Description Test {i}",
            });
        }
        return toDoListMock;
    }
}
