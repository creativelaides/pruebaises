import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TariffsApiService } from '../services/tariffs-api.service';
import { TariffSummary } from '../models/tariff.model';
import { CopCurrencyPipe } from '../../shared/pipes/cop-currency.pipe';

@Component({
  selector: 'app-tariffs-page',
  standalone: true,
  imports: [FormsModule, CopCurrencyPipe],
  templateUrl: './tariffs.page.html',
})
export class TariffsPage {
  private readonly api = inject(TariffsApiService);
  private readonly router = inject(Router);

  protected readonly loading = signal(false);
  protected readonly latest = signal<TariffSummary | null>(null);
  protected readonly tariffs = signal<TariffSummary[]>([]);
  protected readonly selectedTariff = signal<TariffSummary | null>(null);
  protected readonly showDetailsModal = signal(false);
  protected readonly message = signal('');
  protected readonly currentPage = signal(1);
  protected readonly pageSize = 12;
  protected pageInput = 1;

  protected yearInput = '';
  protected period = '';
  protected tariffOperator = '';
  protected level = '';

  protected readonly totalPages = computed(() => {
    const total = this.tariffs().length;
    return Math.max(1, Math.ceil(total / this.pageSize));
  });

  protected readonly pagedTariffs = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    return this.tariffs().slice(start, start + this.pageSize);
  });

  protected readonly periodOptions = computed(() =>
    Array.from(new Set(this.tariffs().map((t) => t.period).filter(Boolean))).sort());

  protected readonly operatorOptions = computed(() =>
    Array.from(new Set(this.tariffs().map((t) => t.tariffOperator).filter(Boolean))).sort());

  protected readonly levelOptions = computed(() =>
    Array.from(new Set(this.tariffs().map((t) => t.level).filter(Boolean))).sort());

  constructor() {
    this.loadAll();
    this.loadLatest();
  }

  protected loadAll(): void {
    this.loading.set(true);
    this.api.getAll(1, 500).subscribe({
      next: (res) => { this.tariffs.set(res.tariffs); this.currentPage.set(1); this.pageInput = 1; this.loading.set(false); },
      error: () => { this.message.set('No se pudieron cargar las tarifas.'); this.loading.set(false); },
    });
  }

  protected selectTariff(id: string): void {
    this.api.getById(id).subscribe({
      next: (item) => {
        this.selectedTariff.set(item);
        this.showDetailsModal.set(true);
      },
      error: () => {
        this.selectedTariff.set(null);
        this.showDetailsModal.set(false);
      },
    });
  }

  protected closeDetails(): void {
    this.showDetailsModal.set(false);
  }

  protected goToSimulation(): void {
    const id = this.selectedTariff()?.id;
    this.showDetailsModal.set(false);
    if (!id) return;
    void this.router.navigate(['/invoices'], { queryParams: { tariffId: id } });
  }

  protected loadLatest(): void {
    this.api.getLatest().subscribe({ next: (res) => this.latest.set(res), error: () => this.latest.set(null) });
  }

  protected searchByPeriod(): void {
    this.loading.set(true);
    this.message.set('');

    const parsedYear = this.yearInput.trim() ? Number(this.yearInput.trim()) : null;
    if (parsedYear !== null && Number.isNaN(parsedYear)) {
      this.loading.set(false);
      this.message.set('El año debe ser numérico.');
      return;
    }

    this.api.getByFilters({
      year: parsedYear,
      period: this.period,
      tariffOperator: this.tariffOperator,
      level: this.level,
    }).subscribe({
      next: (res) => {
        this.tariffs.set(res.tariffs);
        this.currentPage.set(1);
        this.pageInput = 1;
        this.loading.set(false);
        if (res.tariffs.length === 0) this.message.set('No hay resultados para los filtros solicitados.');
      },
      error: (err) => { this.message.set(err?.error?.detail ?? 'No se pudo consultar con los filtros.'); this.loading.set(false); },
    });
  }

  protected reset(): void {
    this.yearInput = '';
    this.period = '';
    this.tariffOperator = '';
    this.level = '';
    this.message.set('');
    this.selectedTariff.set(null);
    this.showDetailsModal.set(false);
    this.pageInput = 1;
    this.loadAll();
    this.loadLatest();
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
