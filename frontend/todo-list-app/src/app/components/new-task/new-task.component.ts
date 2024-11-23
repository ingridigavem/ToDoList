import { Component } from '@angular/core';
import { CirclePlus, LucideAngularModule } from 'lucide-angular';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

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

  constructor(private formBuilder: FormBuilder) {
    this.newTaskForm = this.formBuilder.group({
      newTask: ['', Validators.required],
    });
  }

  handleSubmitNewTask() {
    console.log('submit');
  }
}

