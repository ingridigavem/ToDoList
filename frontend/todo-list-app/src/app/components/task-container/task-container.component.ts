import { animate, style, transition, trigger } from '@angular/animations';
import { HttpStatusCode } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Task } from '../../interfaces/interfaces';
import { ToDoService } from '../../services/to-do.service';
import { EmptyTaskComponent } from '../empty-task/empty-task.component';
import { TaskCardComponent } from '../task-card/task-card.component';

@Component({
  selector: 'app-task-container',
  standalone: true,
  imports: [TaskCardComponent, EmptyTaskComponent],
  templateUrl: './task-container.component.html',
  styleUrl: './task-container.component.scss',
  animations: [
    trigger('fadeOut', [
      transition('* => void', [
        animate(
          '300ms ease-in',
          style({
            opacity: 0,
            transform: 'translateY(-20px)',
          })
        ),
      ]),
    ]),
    trigger('fadeIn', [
      transition('void => *', [
        style({
          opacity: 0,
          transform: 'translateY(-20px)',
        }),
        animate(
          '300ms ease-out',
          style({
            opacity: 1,
            transform: 'translateY(0)',
          })
        ),
      ]),
    ]),
  ],
})
export class TaskContainerComponent implements OnInit {
  toDoService = inject(ToDoService);
  tasks: Task[] = [];

  get activeTasks(): Task[] {
    return this.tasks.filter((t) => !t.deleted);
  }

  ngOnInit(): void {
    this.loadTasks();
    this.toDoService.getTasksObservable().subscribe(() => {
      this.loadTasks();
    });
  }

  handleTaskDelete(taskId: string): void {
    setTimeout(() => {
      this.toDoService.deleteTask(taskId).subscribe((response) => {
        const statusCode = response.status;

        if (statusCode !== HttpStatusCode.NoContent) return;

        this.loadTasks();
      });
    }, 300);
  }

  handleTaskChecked(taskId: string): void {
    this.toDoService.checkTask(taskId).subscribe((response) => {
      const statusCode = response.status;
      const responseBody = response.body;

      if (statusCode !== HttpStatusCode.Ok) return;

      if (responseBody?.data?.success) {
        this.loadTasks();
      }
    });
  }

  loadTasks() {
    this.toDoService.getTasks().subscribe((response) => {
      const statusCode = response.status;
      const responseBody = response.body;

      if (statusCode !== HttpStatusCode.Ok) return;

      this.tasks = responseBody?.data?.toDos || [];
    });
  }
}
