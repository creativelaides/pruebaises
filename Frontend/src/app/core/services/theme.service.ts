import { Injectable, signal } from '@angular/core';

type AppTheme = 'winter' | 'sunset';
const THEME_KEY = 'app.theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly themeState = signal<AppTheme>(this.readInitialTheme());
  readonly theme = this.themeState.asReadonly();

  constructor() { this.apply(this.themeState()); }

  setTheme(theme: AppTheme): void {
    this.themeState.set(theme);
    localStorage.setItem(THEME_KEY, theme);
    this.apply(theme);
  }

  toggle(): void { this.setTheme(this.themeState() === 'winter' ? 'sunset' : 'winter'); }

  private apply(theme: AppTheme): void { document.documentElement.setAttribute('data-theme', theme); }
  private readInitialTheme(): AppTheme { return localStorage.getItem(THEME_KEY) === 'sunset' ? 'sunset' : 'winter'; }
}
