import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { SessionService } from './core/services/session.service';
import { ThemeService } from './core/services/theme.service';
import { ToastCenterComponent } from './shared/components/toast-center.component';
import { NotificationService } from './core/services/notification.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, ToastCenterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly session = inject(SessionService);
  private readonly notifications = inject(NotificationService);
  protected readonly theme = inject(ThemeService);

  protected readonly isAuthenticated = this.session.isAuthenticated;
  protected readonly isAdmin = computed(() => this.session.isAdmin());
  protected readonly displayName = computed(() => {
    const profile = this.session.session()?.profile;
    if (!profile) return this.session.session()?.username ?? 'Usuario';
    const fullName = `${profile.firstName} ${profile.lastName}`.trim();
    return fullName || profile.email || this.session.session()?.username || 'Usuario';
  });
  protected readonly initials = computed(() => {
    const parts = this.displayName().split(/\s+/).filter(Boolean).slice(0, 2);
    return (parts.map((part) => part[0]?.toUpperCase() ?? '').join('') || 'US');
  });

  protected logout(): void {
    this.session.clear();
    this.notifications.info('Sesion cerrada.');
  }
}
