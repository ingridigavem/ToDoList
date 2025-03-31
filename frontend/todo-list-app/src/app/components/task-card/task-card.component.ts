import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LucideAngularModule, Trash2 } from 'lucide-angular';
import { Task } from '../../interfaces/interfaces';

@Component({
  selector: 'app-task-card',
  standalone: true,
  imports: [LucideAngularModule],
  templateUrl: './task-card.component.html',
  styleUrl: './task-card.component.scss',
})
export class TaskCardComponent {
  @Input({
    required: true,
  })
  task!: Task;

  @Output() delete = new EventEmitter<string>();
  @Output() check = new EventEmitter<string>();

  protected readonly TrashIcon = Trash2;

  handleTaskChecked() {
    this.check.emit(this.task.id);
  }

  handleTaskDeleted() {
    this.delete.emit(this.task.id);
  }
}
