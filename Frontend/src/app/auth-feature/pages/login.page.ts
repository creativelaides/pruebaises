import { Component, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AuthApiService } from '../services/auth-api.service';
import { SessionService } from '../../core/services/session.service';
import { NotificationService } from '../../core/services/notification.service';
import { extractRolesFromJwt } from '../../core/utils/jwt.utils';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.page.html',
  styleUrl: './login.page.css',
})
export class LoginPage {
  private readonly fb = inject(FormBuilder);
  private readonly authApi = inject(AuthApiService);
  private readonly session = inject(SessionService);
  private readonly notifications = inject(NotificationService);
  private readonly router = inject(Router);

  protected readonly loading = signal(false);
  protected readonly errorMessage = signal('');

  protected readonly form = this.fb.nonNullable.group({
    email: ['creativelaides@gmail.com', [Validators.required]],
    password: ['HelloWorldHorse9876*', [Validators.required]],
  });

  protected readonly isSubmitDisabled = computed(() => this.form.invalid || this.loading());

  protected submit(): void {
    if (this.form.invalid || this.loading()) return;

    this.errorMessage.set('');
    this.loading.set(true);

    const request = this.form.getRawValue();
    this.authApi.login(request)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          const roles = extractRolesFromJwt(response.accessToken);
          const baseSession = {
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
            tokenType: response.tokenType,
            expiresIn: response.expiresIn,
            username: request.email,
            roles,
          };
          this.session.setSession(baseSession);

          this.authApi.getProfile().subscribe({
            next: (profile) => {
              this.session.setSession({
                ...baseSession,
                username: profile.email ?? baseSession.username,
                roles: profile.roles ?? roles,
                profile: {
                  firstName: profile.firstName ?? '',
                  lastName: profile.lastName ?? '',
                  email: profile.email ?? baseSession.username,
                  jobTitle: profile.jobTitle ?? '',
                  area: profile.area ?? '',
                },
              });
              this.notifications.success('Sesion iniciada correctamente.');
              void this.router.navigateByUrl('/auth/session');
            },
            error: () => {
              this.notifications.success('Sesion iniciada correctamente.');
              void this.router.navigateByUrl('/auth/session');
            },
          });
        },
        error: (error) => {
          this.errorMessage.set(error?.error?.detail ?? 'No se pudo iniciar sesión.');
        },
      });
  }
}
