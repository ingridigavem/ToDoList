import { Component } from '@angular/core';
import { ListPlus, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-empty-task',
  standalone: true,
  imports: [
    LucideAngularModule
  ],
  templateUrl: './empty-task.component.html',
  styleUrl: './empty-task.component.scss'
})
export class EmptyTaskComponent {

  protected readonly ClipboardList = ListPlus;
}



