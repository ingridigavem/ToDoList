import { Component, inject } from '@angular/core';
import { CirclePlus, LucideAngularModule } from 'lucide-angular';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToDoService } from '../../services/to-do.service';

@Component({
  selector: 'app-new-task',
  standalone: true,
  imports: [LucideAngularModule, ReactiveFormsModule],
  templateUrl: './new-task.component.html',
  styleUrl: './new-task.component.scss'
})
export class NewTaskComponent {
  readonly CirclePlus = CirclePlus;
  newTaskForm: FormGroup;
  toDoService = inject(ToDoService);

  constructor(private formBuilder: FormBuilder) {
    this.newTaskForm = this.formBuilder.group({
      description: ['', Validators.required],
    });
  }

  handleSubmitNewTask() {
    if (this.newTaskForm.valid) this.toDoService.insertTask(this.newTaskForm.value.description);
    this.newTaskForm.reset();
  }
}

