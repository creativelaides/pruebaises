import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-base-url.token';
import { CreateTariffRequest, TariffByPeriodResponse, TariffListResponse, TariffSummary, UpdateTariffRequest } from '../models/tariff.model';

@Injectable({ providedIn: 'root' })
export class TariffsApiService {
  constructor(private readonly http: HttpClient, @Inject(API_BASE_URL) private readonly apiBaseUrl: string) {}

  getAll(page = 1, pageSize = 50): Observable<TariffListResponse> {
    return this.http.get<TariffListResponse>(`${this.apiBaseUrl}/api/tariffs?page=${page}&pageSize=${pageSize}`);
  }

  getLatest(): Observable<TariffSummary> { return this.http.get<TariffSummary>(`${this.apiBaseUrl}/api/tariffs/latest`); }
  getById(id: string): Observable<TariffSummary> { return this.http.get<TariffSummary>(`${this.apiBaseUrl}/api/tariffs/${id}`); }

  getByFilters(filters: { year?: number | null; period?: string; tariffOperator?: string; level?: string }): Observable<TariffByPeriodResponse> {
    const params = new URLSearchParams();
    if (filters.year !== null && filters.year !== undefined) params.set('year', String(filters.year));
    if (filters.period?.trim()) params.set('period', filters.period.trim());
    if (filters.tariffOperator?.trim()) params.set('tariffOperator', filters.tariffOperator.trim());
    if (filters.level?.trim()) params.set('level', filters.level.trim());

    const query = params.toString();
    const url = query ? `${this.apiBaseUrl}/api/tariffs/by-period?${query}` : `${this.apiBaseUrl}/api/tariffs/by-period`;
    return this.http.get<TariffByPeriodResponse>(url);
  }

  create(request: CreateTariffRequest): Observable<TariffSummary> { return this.http.post<TariffSummary>(`${this.apiBaseUrl}/api/tariffs`, request); }
  update(id: string, request: UpdateTariffRequest): Observable<TariffSummary> { return this.http.put<TariffSummary>(`${this.apiBaseUrl}/api/tariffs/${id}`, request); }
  delete(id: string): Observable<{ id: string; success: boolean; message: string }> { return this.http.delete<{ id: string; success: boolean; message: string }>(`${this.apiBaseUrl}/api/tariffs/${id}`); }
}
