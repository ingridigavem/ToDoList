import { Injectable } from '@angular/core';
import { Task } from '../interfaces/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ToDoService {
  private tasks: Task[] = [
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

  constructor() {
  }

  getTasks() {
    return this.tasks;
  }

  insertTask(description: string) {
    const lastId = this.tasks.length > 0 ? Math.max(...this.tasks.map(task => task.id)) : 0;

    const task: Task = {
      id: lastId + 1,
      description: description,
      checked: false,
      deleted: false
    };

    this.tasks.push(task);
  }

  deleteTask(id: number) {
    this.tasks.find(task => task.id === id)!.deleted = true;
  }

  checkTask(id: number) {
    const task = this.tasks.find(task => task.id === id)!;
    task.checked = !task.checked;
  }
}
