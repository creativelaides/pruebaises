import { Component, inject } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-toast-center',
  standalone: true,
  templateUrl: './toast-center.component.html',
})
export class ToastCenterComponent {
  protected readonly notifications = inject(NotificationService);
}
