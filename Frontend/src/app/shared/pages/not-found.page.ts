import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="hero min-h-[60vh]">
      <div class="hero-content text-center">
        <div>
          <h1 class="text-5xl font-bold">404</h1>
          <p class="py-4">No encontramos la página solicitada.</p>
          <a class="btn btn-primary" routerLink="/tariffs">Ir a inicio</a>
        </div>
      </div>
    </div>
  `,
})
export class NotFoundPage {}
