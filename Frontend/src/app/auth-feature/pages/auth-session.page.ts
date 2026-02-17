import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AuthApiService } from '../services/auth-api.service';
import { SessionService } from '../../core/services/session.service';
import { NotificationService } from '../../core/services/notification.service';
import { UserProfile } from '../models/profile.model';

@Component({
  selector: 'app-auth-session-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './auth-session.page.html',
})
export class AuthSessionPage {
  protected readonly session = inject(SessionService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);
  private readonly authApi = inject(AuthApiService);
  private readonly fb = inject(FormBuilder);

  protected readonly loading = signal(false);
  protected readonly savingProfile = signal(false);
  protected readonly changingPassword = signal(false);
  protected readonly profile = signal<UserProfile | null>(null);

  protected readonly profileForm = this.fb.nonNullable.group({
    firstName: [''],
    lastName: [''],
    email: ['', [Validators.required]],
    jobTitle: [''],
    area: [''],
  });

  protected readonly passwordForm = this.fb.nonNullable.group({
    currentPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
  });

  protected readonly profileDisplayName = computed(() => {
    const profile = this.profile();
    if (!profile) return this.session.session()?.username ?? 'Usuario';
    const fullName = [profile.firstName, profile.lastName].filter(Boolean).join(' ').trim();
    return fullName || profile.email || profile.userName || 'Usuario';
  });

  protected readonly avatarInitials = computed(() => {
    const name = this.profileDisplayName().trim();
    if (!name) return 'US';
    const parts = name.split(/\s+/).slice(0, 2);
    return parts.map((part) => part[0]?.toUpperCase() ?? '').join('') || 'US';
  });

  constructor() {
    this.loadProfile();
  }

  protected loadProfile(): void {
    this.loading.set(true);
    this.authApi.getProfile()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (profile) => {
          this.profile.set(profile);
          this.profileForm.patchValue({
            firstName: profile.firstName ?? '',
            lastName: profile.lastName ?? '',
            email: profile.email ?? '',
            jobTitle: profile.jobTitle ?? '',
            area: profile.area ?? '',
          });
          this.session.updateProfile({
            firstName: profile.firstName ?? '',
            lastName: profile.lastName ?? '',
            email: profile.email ?? '',
            jobTitle: profile.jobTitle ?? '',
            area: profile.area ?? '',
          });
          const currentSession = this.session.session();
          if (currentSession) {
            this.session.setSession({
              ...currentSession,
              roles: profile.roles ?? currentSession.roles,
            });
          }
        },
        error: (error) => {
          this.notifications.error(error?.error?.detail ?? 'No se pudo cargar el perfil.');
        },
      });
  }

  protected saveProfile(): void {
    if (this.profileForm.invalid || this.savingProfile()) return;
    this.savingProfile.set(true);

    this.authApi.updateProfile(this.profileForm.getRawValue())
      .pipe(finalize(() => this.savingProfile.set(false)))
      .subscribe({
        next: (profile) => {
          this.profile.set(profile);
          const currentSession = this.session.session();
          if (currentSession) {
            const email = profile.email ?? currentSession.username;
            this.session.setSession({
              ...currentSession,
              username: email,
              profile: {
                firstName: profile.firstName ?? '',
                lastName: profile.lastName ?? '',
                email,
                jobTitle: profile.jobTitle ?? '',
                area: profile.area ?? '',
              },
            });
          }
          this.notifications.success('Perfil actualizado.');
        },
        error: (error) => this.notifications.error(error?.error?.detail ?? 'No se pudo actualizar el perfil.'),
      });
  }

  protected updatePassword(): void {
    if (this.passwordForm.invalid || this.changingPassword()) return;

    const { currentPassword, newPassword, confirmPassword } = this.passwordForm.getRawValue();
    if (newPassword !== confirmPassword) {
      this.notifications.warning('La confirmacion de contrasena no coincide.');
      return;
    }

    this.changingPassword.set(true);
    this.authApi.changePassword({ currentPassword, newPassword })
      .pipe(finalize(() => this.changingPassword.set(false)))
      .subscribe({
        next: () => {
          this.passwordForm.reset();
          this.notifications.success('Contrasena actualizada correctamente.');
        },
        error: (error) => this.notifications.error(error?.error?.detail ?? 'No se pudo cambiar la contrasena.'),
      });
  }

  protected logout(): void {
    this.session.clear();
    void this.router.navigateByUrl('/auth/login');
  }
}
