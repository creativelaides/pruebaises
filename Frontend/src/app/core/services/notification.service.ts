import { Injectable, signal } from '@angular/core';
import { ToastMessage, ToastType } from '../models/toast-message.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly queue = signal<ToastMessage[]>([]);
  readonly messages = this.queue.asReadonly();

  success(text: string): void { this.push(text, 'success'); }
  error(text: string): void { this.push(text, 'error'); }
  warning(text: string): void { this.push(text, 'warning'); }
  info(text: string): void { this.push(text, 'info'); }

  dismiss(id: string): void {
    this.queue.update((messages) => messages.filter((m) => m.id !== id));
  }

  private push(text: string, type: ToastType): void {
    const id = crypto.randomUUID();
    this.queue.update((messages) => [...messages, { id, text, type }]);
    setTimeout(() => this.dismiss(id), 3500);
  }
}
