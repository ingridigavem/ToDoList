﻿using MediatR;
using ToDoList.Domain.Shared.DTOs;

namespace ToDoList.Domain.ToDoList.UseCases.GetAllToDos;
public record GetAllToDosRequest(int PageNumber = 0, int PageSize = 20) : IRequest<Result<GetAllToDosResponse>>;