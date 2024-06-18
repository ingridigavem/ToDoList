namespace ToDoList.Domain.Shared.DTOs;
public class Result<T> {
    public T? Data { get; private set; }
    public List<string> Errors { get; private set; } = new List<string>();
    public string? ExceptionMessage { get; private set; }

    public Result(T data, List<string> errors) {
        Data = data;
        Errors = errors;
    }

    public Result(T data) {
        Data = data;
    }

    public Result(List<string> errors) {
        Errors = errors;
    }

    public Result(string error) {
        Errors.Add(error);
    }

    public Result(string error, string exceptionMessage) {
        Errors.Add(error);
        ExceptionMessage = exceptionMessage;
    }
}
