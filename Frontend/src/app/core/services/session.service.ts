import { Injectable, computed, signal } from '@angular/core';
import { SessionUser } from '../models/session-user.model';

const STORAGE_KEY = 'auth.session';

@Injectable({ providedIn: 'root' })
export class SessionService {
  private readonly sessionState = signal<SessionUser | null>(this.readFromStorage());
  readonly session = this.sessionState.asReadonly();
  readonly isAuthenticated = computed(() => this.sessionState() !== null);

  setSession(session: SessionUser): void { this.sessionState.set(session); localStorage.setItem(STORAGE_KEY, JSON.stringify(session)); }
  updateProfile(profile: SessionUser['profile']): void {
    const current = this.sessionState();
    if (!current) return;
    const updated = { ...current, profile };
    this.setSession(updated);
  }

  clear(): void { this.sessionState.set(null); localStorage.removeItem(STORAGE_KEY); }
  token(): string | null { return this.sessionState()?.accessToken ?? null; }
  roles(): string[] { return this.sessionState()?.roles ?? []; }
  isAdmin(): boolean { return this.roles().some((role) => role.toLowerCase() === 'admin'); }

  private readFromStorage(): SessionUser | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return null;
    try { return JSON.parse(raw) as SessionUser; } catch { localStorage.removeItem(STORAGE_KEY); return null; }
  }
}
