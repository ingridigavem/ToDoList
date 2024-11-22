import { Component, Input } from '@angular/core';
import { LucideAngularModule, Trash2 } from 'lucide-angular';
import { Task } from '../../interfaces/interfaces';

@Component({
  selector: 'app-task-card',
  standalone: true,
  imports: [LucideAngularModule],
  templateUrl: './task-card.component.html',
  styleUrl: './task-card.component.scss'
})
export class TaskCardComponent {
  @Input({
    required: true,
  }) task!: Task;

  protected readonly TrashIcon = Trash2;

  checkTask(event: Event) {
    console.log(event);
    this.task.checked = !this.task.checked;
  }

  deleteTask(taskId: number) {
    console.log(taskId);
  }
}
