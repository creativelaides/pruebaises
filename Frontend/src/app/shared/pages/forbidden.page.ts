import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-forbidden-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="hero min-h-[60vh]">
      <div class="hero-content text-center">
        <div>
          <h1 class="text-5xl font-bold text-error">403</h1>
          <p class="py-4">No tienes permisos para esta sección.</p>
          <a class="btn btn-primary" routerLink="/tariffs">Volver</a>
        </div>
      </div>
    </div>
  `,
})
export class ForbiddenPage {}
