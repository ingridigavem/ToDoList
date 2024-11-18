import { Component } from '@angular/core';
import { TaskCardComponent } from '../task-card/task-card.component';
import { EmptyTaskComponent } from '../empty-task/empty-task.component';

@Component({
  selector: 'app-task-container',
  standalone: true,
  imports: [TaskCardComponent, EmptyTaskComponent],
  templateUrl: './task-container.component.html',
  styleUrl: './task-container.component.scss'
})
export class TaskContainerComponent {
  tasks = [1, 2, 3];
}
