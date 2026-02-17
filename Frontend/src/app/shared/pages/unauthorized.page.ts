import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-unauthorized-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="hero min-h-[60vh]">
      <div class="hero-content text-center">
        <div>
          <h1 class="text-5xl font-bold text-warning">401</h1>
          <p class="py-4">Debes iniciar sesión para acceder.</p>
          <a class="btn btn-primary" routerLink="/auth/login">Ir a login</a>
        </div>
      </div>
    </div>
  `,
})
export class UnauthorizedPage {}
