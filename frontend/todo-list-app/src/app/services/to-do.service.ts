import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ApiResponse,
  CompleteToDoResponse,
  GetAllToDosResponse,
} from '../types/types';

@Injectable({
  providedIn: 'root',
})
export class ToDoService {
  private serverUrl = environment.toDoApiV1Url;

  private tasksSubject = new Subject<any>();

  constructor(private http: HttpClient) {}

  getTasks() {
    return this.http.get<ApiResponse<GetAllToDosResponse>>(
      `${this.serverUrl}/toDos`,
      { observe: 'response' }
    );
  }

  insertTask(description: string) {
    return this.http
      .post<ApiResponse<null>>(
        `${this.serverUrl}/toDo`,
        { description },
        {
          observe: 'response',
        }
      )
      .pipe(
        tap(() => {
          this.tasksSubject.next(null);
        })
      );
  }

  getTasksObservable() {
    return this.tasksSubject.asObservable();
  }

  deleteTask(id: string) {
    return this.http.delete<ApiResponse<null>>(`${this.serverUrl}/toDo/${id}`, {
      observe: 'response',
    });
  }

  checkTask(id: string) {
    return this.http.patch<ApiResponse<CompleteToDoResponse>>(
      `${this.serverUrl}/toDo/complete/${id}`,
      {},
      { observe: 'response' }
    );
  }
}
