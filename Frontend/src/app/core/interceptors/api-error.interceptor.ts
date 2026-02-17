import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';
import { SessionService } from '../services/session.service';

export const apiErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const notifications = inject(NotificationService);
  const session = inject(SessionService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        session.clear();
        notifications.warning('Sesión expirada o credenciales inválidas.');
        void router.navigateByUrl('/401');
      } else if (error.status === 403) {
        notifications.warning('No tienes permisos para esta acción.');
        void router.navigateByUrl('/403');
      } else if (error.status === 404 && !req.url.includes('/api/tariffs/by-period')) {
        void router.navigateByUrl('/404');
      } else if (error.status >= 500) {
        notifications.error('Error interno del servidor.');
      }

      return throwError(() => error);
    }),
  );
};
