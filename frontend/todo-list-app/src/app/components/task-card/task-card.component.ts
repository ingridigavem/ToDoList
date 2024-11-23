import { Component, EventEmitter, Input, Output } from '@angular/core';
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

  @Output() delete = new EventEmitter<number>();
  @Output() check = new EventEmitter<number>();
  
  protected readonly TrashIcon = Trash2;

  handleTaskCheck() {
    this.check.emit(this.task.id);
  }

  onDelete() {
    this.delete.emit(this.task.id);
  }
}
