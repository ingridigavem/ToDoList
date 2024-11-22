import { Component } from '@angular/core';
import { TaskCardComponent } from '../task-card/task-card.component';
import { EmptyTaskComponent } from '../empty-task/empty-task.component';
import { Task } from '../../interfaces/interfaces';

@Component({
  selector: 'app-task-container',
  standalone: true,
  imports: [TaskCardComponent, EmptyTaskComponent],
  templateUrl: './task-container.component.html',
  styleUrl: './task-container.component.scss'
})
export class TaskContainerComponent {
  tasks: Task[] = [
    {
      id: 1,
      description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Alias asperiores nam necessitatibus obcaecati rem temporibus.',
      checked: false,
      deleted: false
    },
    {
      id: 2,
      description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Alias asperiores nam necessitatibus obcaecati rem temporibus.',
      checked: true,
      deleted: false
    },
    {
      id: 3,
      description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Alias asperiores nam necessitatibus obcaecati rem temporibus.',
      checked: false,
      deleted: false
    }
  ];

  tasksNotDeleted: Task[] = this.tasks.filter(t => !t.deleted);
}
