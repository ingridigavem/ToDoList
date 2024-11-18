import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NewTaskComponent } from './components/new-task/new-task.component';
import { TaskContainerComponent } from './components/task-container/task-container.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NewTaskComponent, TaskContainerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
}
