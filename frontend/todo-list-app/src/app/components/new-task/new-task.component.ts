import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CirclePlus, LucideAngularModule } from 'lucide-angular';
import { ToDoService } from '../../services/to-do.service';

@Component({
  selector: 'app-new-task',
  standalone: true,
  imports: [LucideAngularModule, ReactiveFormsModule],
  templateUrl: './new-task.component.html',
  styleUrl: './new-task.component.scss',
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
    if (this.newTaskForm.valid)
      this.toDoService
        .insertTask(this.newTaskForm.value.description)
        .subscribe((response) => {
          if (response.status !== 201) return;

          const location = response.headers.get('Location');
          console.log('Task criada! URL:', location);
        });
    this.newTaskForm.reset();
  }
}
