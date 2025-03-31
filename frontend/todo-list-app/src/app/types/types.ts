import { HttpStatusCode } from '@angular/common/http';
import { Task } from '../interfaces/interfaces';

export type ApiResponse<T = object> = {
  data: T | null;
  status: HttpStatusCode;
  errors: string[];
  exceptionMessage: string | null;
  hasErrors: boolean;
  success: boolean;
};

export type GetAllToDosResponse = {
  toDos: Task[] | null;
  totalCount: number;
};

export type CompleteToDoResponse = {
  success: boolean;
};
