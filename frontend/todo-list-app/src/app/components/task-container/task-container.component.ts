import { Component, inject, OnInit } from '@angular/core';
import { TaskCardComponent } from '../task-card/task-card.component';
import { EmptyTaskComponent } from '../empty-task/empty-task.component';
import { Task } from '../../interfaces/interfaces';
import { ToDoService } from '../../services/to-do.service';
import { animate, style, transition, trigger } from '@angular/animations';

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
    return this.tasks.filter(t => !t.deleted);
  }

  ngOnInit() {
    this.tasks = this.toDoService.getTasks();
  }

  handleTaskDelete(taskId: number): void {
    setTimeout(() => {
      this.toDoService.deleteTask(taskId);
    }, 300);
  }

  handleTaskChecked(taskId: number): void {
    this.toDoService.checkTask(taskId);
  }
}
