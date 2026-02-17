import { Component, computed, inject, signal } from '@angular/core';
import { EtlApiService } from '../services/etl-api.service';
import { ExecuteEtlResponse } from '../models/etl.model';
import { NotificationService } from '../../core/services/notification.service';
import { SessionService } from '../../core/services/session.service';
import { EtlAdminVisualComponent } from '../components/etl-admin-visual.component';

@Component({
  selector: 'app-etl-page',
  standalone: true,
  imports: [EtlAdminVisualComponent],
  templateUrl: './etl.page.html',
})
export class EtlPage {
  private readonly api = inject(EtlApiService);
  private readonly notifications = inject(NotificationService);
  private readonly session = inject(SessionService);

  protected readonly loading = signal(false);
  protected readonly error = signal('');
  protected readonly result = signal<ExecuteEtlResponse | null>(null);
  protected readonly isAdmin = computed(() => this.session.isAdmin());

  protected run(): void {
    this.loading.set(true);
    this.error.set('');
    this.api.run().subscribe({
      next: (res) => {
        this.result.set(res);
        this.notifications.success('ETL ejecutado.');
        this.loading.set(false);
      },
      error: (err) => {
        const msg = err?.error?.detail ?? 'No se pudo ejecutar ETL.';
        this.error.set(msg);
        this.notifications.error(msg);
        this.loading.set(false);
      },
    });
  }
}
