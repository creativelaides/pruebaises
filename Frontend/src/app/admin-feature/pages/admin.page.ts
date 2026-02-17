import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TariffsApiService } from '../../tariffs-feature/services/tariffs-api.service';
import { CreateTariffRequest, TariffSummary, UpdateTariffRequest } from '../../tariffs-feature/models/tariff.model';
import { NotificationService } from '../../core/services/notification.service';
import { EmailApiService } from '../services/email-api.service';
import { CopCurrencyPipe } from '../../shared/pipes/cop-currency.pipe';

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [FormsModule, CopCurrencyPipe],
  templateUrl: './admin.page.html',
})
export class AdminPage {
  private readonly api = inject(TariffsApiService);
  private readonly emailApi = inject(EmailApiService);
  private readonly notifications = inject(NotificationService);

  protected readonly tariffs = signal<TariffSummary[]>([]);
  protected readonly companyOptions = computed(() => {
    const map = new Map<string, { companyId: string; tariffOperator: string }>();
    for (const tariff of this.tariffs()) {
      if (!map.has(tariff.companyId)) {
        map.set(tariff.companyId, { companyId: tariff.companyId, tariffOperator: tariff.tariffOperator });
      }
    }

    return Array.from(map.values());
  });
  protected readonly months = [
    'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
    'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre',
  ];
  protected readonly currentPage = signal(1);
  protected readonly pageSize = 12;
  protected pageInput = 1;
  protected readonly totalPages = computed(() => {
    const total = this.tariffs().length;
    return Math.max(1, Math.ceil(total / this.pageSize));
  });
  protected readonly pagedTariffs = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    return this.tariffs().slice(start, start + this.pageSize);
  });
  protected readonly message = signal('');
  protected readonly error = signal('');
  protected emailTo = 'creativelaides@gmail.com';

  protected createModel: CreateTariffRequest = {
    year: new Date().getFullYear(),
    period: 'Enero',
    level: 'Nivel 1 (Propiedad OR)',
    tariffOperator: 'Operador manual',
    companyId: '',
    totalCu: null,
    purchaseCostG: null,
    chargeTransportStnTm: null,
    chargeTransportSdlDm: null,
    marketingMargin: null,
    costLossesPr: null,
    restrictionsRm: null,
    cot: null,
    cfmjGfact: null,
  };

  protected updateTariffId = '';
  protected updateModel: UpdateTariffRequest = {
    totalCu: null,
    purchaseCostG: null,
    chargeTransportStnTm: null,
    chargeTransportSdlDm: null,
    marketingMargin: null,
    costLossesPr: null,
    restrictionsRm: null,
    cot: null,
    cfmjGfact: null,
  };

  constructor() { this.refresh(); }

  protected refresh(): void {
    this.error.set('');
    this.api.getAll(1, 500).subscribe({
      next: (res) => {
        this.tariffs.set(res.tariffs);
        this.currentPage.set(1);
        this.pageInput = 1;
        if (!this.createModel.companyId && res.tariffs.length > 0) {
          this.createModel.companyId = res.tariffs[0].companyId;
          this.createModel.tariffOperator = res.tariffs[0].tariffOperator;
        }
      },
      error: (err) => {
        const msg = this.getErrorMessage(err, 'No se pudo cargar tarifas para administración.');
        this.error.set(msg);
        this.notifications.error(msg);
      },
    });
  }

  protected create(): void {
    this.message.set('');
    this.error.set('');
    const payload: CreateTariffRequest = {
      ...this.createModel,
      totalCu: this.asNumber(this.createModel.totalCu),
      purchaseCostG: this.asNumber(this.createModel.purchaseCostG),
      chargeTransportStnTm: this.asNumber(this.createModel.chargeTransportStnTm),
      chargeTransportSdlDm: this.asNumber(this.createModel.chargeTransportSdlDm),
      marketingMargin: this.asNumber(this.createModel.marketingMargin),
      costLossesPr: this.asNumber(this.createModel.costLossesPr),
      restrictionsRm: this.asNumber(this.createModel.restrictionsRm),
      cot: this.asNumber(this.createModel.cot),
      cfmjGfact: this.asNumber(this.createModel.cfmjGfact),
    };

    this.api.create(payload).subscribe({
      next: (res) => {
        this.message.set(`Tarifa creada: ${res.id}`);
        this.notifications.success('Tarifa creada correctamente.');
        this.refresh();
      },
      error: (err) => {
        const msg = this.getErrorMessage(err, 'No se pudo crear tarifa.');
        this.error.set(msg);
        this.notifications.error(msg);
      },
    });
  }

  protected update(): void {
    this.message.set('');
    this.error.set('');
    if (!this.updateTariffId) { this.error.set('Ingresa el ID de la tarifa a actualizar.'); return; }

    this.api.update(this.updateTariffId, this.updateModel).subscribe({
      next: (res) => {
        this.message.set(`Tarifa actualizada: ${res.id}`);
        this.notifications.success('Tarifa actualizada.');
        this.refresh();
      },
      error: (err) => {
        const msg = this.getErrorMessage(err, 'No se pudo actualizar tarifa.');
        this.error.set(msg);
        this.notifications.error(msg);
      },
    });
  }

  protected remove(id: string): void {
    this.message.set('');
    this.error.set('');
    this.api.delete(id).subscribe({
      next: (res) => {
        this.message.set(res.message);
        this.notifications.success(res.message);
        this.refresh();
      },
      error: (err) => {
        const msg = this.getErrorMessage(err, 'No se pudo eliminar tarifa.');
        this.error.set(msg);
        this.notifications.error(msg);
      },
    });
  }

  protected setCompany(companyId: string): void {
    this.createModel.companyId = companyId;
    const sample = this.companyOptions().find((t) => t.companyId === companyId);
    if (sample) this.createModel.tariffOperator = sample.tariffOperator;
  }

  protected sendTestEmail(): void {
    this.emailApi.sendTest(this.emailTo).subscribe({
      next: (res) => {
        this.message.set(res.message);
        this.notifications.success(res.message);
      },
      error: (err) => {
        const msg = this.getErrorMessage(err, 'No se pudo enviar correo de prueba.');
        this.error.set(msg);
        this.notifications.error(msg);
      },
    });
  }

  private asNumber(value: number | null): number {
    return value ?? 0;
  }

  private getErrorMessage(error: unknown, fallback: string): string {
    const err = error as { error?: { detail?: string } | string; message?: string };
    if (typeof err?.error === 'string' && err.error.trim()) return err.error;
    if (typeof err?.error === 'object' && err.error?.detail) return err.error.detail;
    if (typeof err?.message === 'string' && err.message.trim()) return err.message;
    return fallback;
  }

  protected goToNextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((v) => v + 1);
      this.pageInput = this.currentPage();
    }
  }

  protected goToPreviousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update((v) => v - 1);
      this.pageInput = this.currentPage();
    }
  }

  protected goToPage(): void {
    const target = Number(this.pageInput);
    if (Number.isNaN(target)) return;
    const clamped = Math.min(Math.max(1, target), this.totalPages());
    this.currentPage.set(clamped);
    this.pageInput = clamped;
  }
}
