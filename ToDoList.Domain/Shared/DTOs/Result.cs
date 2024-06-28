using System.Net;

namespace ToDoList.Domain.Shared.DTOs;
public class Result<T> {
    public T? Data { get; private set; }
    public HttpStatusCode Status { get; private set; }
    public List<string> Errors { get; private set; } = [];
    public string? ExceptionMessage { get; private set; }

    public Result(T data, List<string> errors, HttpStatusCode status) {
        Data = data;
        Errors = errors;
        Status = status;
    }

    public Result(T data, HttpStatusCode status) {
        Data = data;
        Status = status;
    }

    public Result(List<string> errors, HttpStatusCode status) {
        Errors = errors;
        Status = status;
    }

    public Result(string error, HttpStatusCode status) {
        Errors.Add(error);
        Status = status;
    }

    public Result(string error, string exceptionMessage, HttpStatusCode status) {
        Errors.Add(error);
        ExceptionMessage = exceptionMessage;
        Status = status;
    }
}
