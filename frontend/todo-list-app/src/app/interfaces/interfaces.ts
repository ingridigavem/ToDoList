export interface Task {
  id: string;
  description: string | null;
  done: boolean;
  deleted: boolean;
  createdAt: Date;
}
