﻿using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Domain.ToDoList.UseCases.Contracts;
public interface IToDoRepository {
    Task SaveToDo(ToDo toDo);
    Task DeleteToDo(Guid id);
    Task<IList<ToDo>> GetAll(int PageNumber = 0, int PageSize = 10);
    Task<int> Count();
}