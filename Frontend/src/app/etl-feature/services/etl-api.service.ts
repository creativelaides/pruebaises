import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-base-url.token';
import { ExecuteEtlResponse } from '../models/etl.model';

@Injectable({ providedIn: 'root' })
export class EtlApiService {
  constructor(private readonly http: HttpClient, @Inject(API_BASE_URL) private readonly apiBaseUrl: string) {}

  run(): Observable<ExecuteEtlResponse> {
    return this.http.post<ExecuteEtlResponse>(`${this.apiBaseUrl}/api/etl/run`, {});
  }
}
