import { Routes } from '@angular/router';
import { authGuard } from '../core/guards/auth.guard';
import { LoginPage } from './pages/login.page';
import { AuthSessionPage } from './pages/auth-session.page';

export const AUTH_ROUTES: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'session', canActivate: [authGuard], component: AuthSessionPage },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
];
