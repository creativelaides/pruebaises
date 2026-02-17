import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-base-url.token';

@Injectable({ providedIn: 'root' })
export class EmailApiService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string,
  ) {}

  sendTest(to: string): Observable<{ message: string }> {
    const encoded = encodeURIComponent(to);
    return this.http.post<{ message: string }>(`${this.apiBaseUrl}/api/email/test?to=${encoded}`, {});
  }
}
