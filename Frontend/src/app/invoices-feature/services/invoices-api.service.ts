import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-base-url.token';
import { SimulateInvoiceRequest, SimulateInvoiceResponse } from '../models/invoice.model';

@Injectable({ providedIn: 'root' })
export class InvoicesApiService {
  constructor(private readonly http: HttpClient, @Inject(API_BASE_URL) private readonly apiBaseUrl: string) {}

  simulate(request: SimulateInvoiceRequest): Observable<SimulateInvoiceResponse> {
    return this.http.post<SimulateInvoiceResponse>(`${this.apiBaseUrl}/api/invoices/simulate`, request);
  }
}
