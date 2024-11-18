import { Component } from '@angular/core';
import { CirclePlus, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-new-task',
  standalone: true,
  imports: [LucideAngularModule],
  templateUrl: './new-task.component.html',
  styleUrl: './new-task.component.scss'
})
export class NewTaskComponent {
  readonly CirclePlus = CirclePlus;
}

