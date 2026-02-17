import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { UnauthorizedPage } from './shared/pages/unauthorized.page';
import { ForbiddenPage } from './shared/pages/forbidden.page';
import { NotFoundPage } from './shared/pages/not-found.page';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tariffs' },
  { path: 'auth', loadChildren: () => import('./auth-feature/auth.routes').then((m) => m.AUTH_ROUTES) },
  { path: 'tariffs', loadChildren: () => import('./tariffs-feature/tariffs.routes').then((m) => m.TARIFFS_ROUTES) },
  { path: 'invoices', canActivate: [authGuard], loadChildren: () => import('./invoices-feature/invoices.routes').then((m) => m.INVOICES_ROUTES) },
  { path: 'etl', canActivate: [authGuard, adminGuard], loadChildren: () => import('./etl-feature/etl.routes').then((m) => m.ETL_ROUTES) },
  { path: 'admin', canActivate: [authGuard, adminGuard], loadChildren: () => import('./admin-feature/admin.routes').then((m) => m.ADMIN_ROUTES) },
  { path: '401', component: UnauthorizedPage },
  { path: '403', component: ForbiddenPage },
  { path: '404', component: NotFoundPage },
  { path: '**', redirectTo: '404' },
];
