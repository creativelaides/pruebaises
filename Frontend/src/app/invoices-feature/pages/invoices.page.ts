import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { InvoicesApiService } from '../services/invoices-api.service';
import { SimulateInvoiceResponse } from '../models/invoice.model';
import { NotificationService } from '../../core/services/notification.service';
import { TariffsApiService } from '../../tariffs-feature/services/tariffs-api.service';
import { TariffSummary } from '../../tariffs-feature/models/tariff.model';
import { buildPolicyNumberFromTariffId } from '../utils/policy-number.util';
import { CopCurrencyPipe } from '../../shared/pipes/cop-currency.pipe';

@Component({
  selector: 'app-invoices-page',
  standalone: true,
  imports: [FormsModule, CopCurrencyPipe],
  templateUrl: './invoices.page.html',
})
export class InvoicesPage {
  private readonly api = inject(InvoicesApiService);
  private readonly tariffsApi = inject(TariffsApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly notifications = inject(NotificationService);

  protected tariffId = '';
  protected kwhConsumption = 120;
  protected readonly tariffs = signal<TariffSummary[]>([]);
  protected readonly selectedTariff = signal<TariffSummary | null>(null);
  protected readonly policyNumber = signal('');

  protected readonly loading = signal(false);
  protected readonly loadingTariffs = signal(false);
  protected readonly error = signal('');
  protected readonly result = signal<SimulateInvoiceResponse | null>(null);

  constructor() {
    this.route.queryParamMap.subscribe((params) => {
      const tariffId = params.get('tariffId');
      if (!tariffId) return;
      this.tariffId = tariffId;
      this.loadTariffDetails();
    });
    this.loadTariffs();
  }

  protected loadTariffs(): void {
    this.loadingTariffs.set(true);
    this.tariffsApi.getAll(1, 100).subscribe({
      next: (response) => {
        this.tariffs.set(response.tariffs);
        this.loadingTariffs.set(false);
        if (this.tariffId) {
          this.loadTariffDetails();
          return;
        }

        const latest = response.tariffs[0];
        if (latest) {
          this.tariffId = latest.id;
          this.loadTariffDetails();
        }
      },
      error: (err) => {
        this.loadingTariffs.set(false);
        this.error.set(err?.error?.detail ?? 'No se pudieron cargar tarifas para simulacion.');
      },
    });
  }

  protected loadTariffDetails(): void {
    if (!this.tariffId) return;
    this.tariffsApi.getById(this.tariffId).subscribe({
      next: (tariff) => {
        this.selectedTariff.set(tariff);
        this.policyNumber.set(buildPolicyNumberFromTariffId(tariff.id));
      },
      error: () => {
        this.selectedTariff.set(null);
        this.policyNumber.set('');
      },
    });
  }

  protected useLatestTariff(): void {
    this.tariffsApi.getLatest().subscribe({
      next: (tariff) => {
        this.tariffId = tariff.id;
        this.selectedTariff.set(tariff);
        this.policyNumber.set(buildPolicyNumberFromTariffId(tariff.id));
        this.notifications.info('Tarifa mas reciente seleccionada.');
      },
      error: () => this.notifications.warning('No se pudo consultar la tarifa mas reciente.'),
    });
  }

  protected simulate(): void {
    this.loading.set(true);
    this.error.set('');
    this.result.set(null);

    this.api.simulate({ tariffId: this.tariffId, kwhConsumption: this.kwhConsumption }).subscribe({
      next: (res) => {
        this.result.set(res);
        this.policyNumber.set(buildPolicyNumberFromTariffId(res.tariffId));
        this.notifications.success('Simulacion completada.');
        this.loading.set(false);
      },
      error: (err) => {
        const msg = err?.error?.detail ?? 'No se pudo simular la factura.';
        this.error.set(msg);
        this.notifications.error(msg);
        this.loading.set(false);
      },
    });
  }
}
