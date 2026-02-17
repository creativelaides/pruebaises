import { CommonModule, DatePipe, PercentPipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ExecuteEtlResponse } from '../models/etl.model';

@Component({
  selector: 'app-etl-admin-visual',
  standalone: true,
  imports: [CommonModule, DatePipe, PercentPipe],
  templateUrl: './etl-admin-visual.component.html',
})
export class EtlAdminVisualComponent {
  @Input() loading = false;
  @Input() error = '';
  @Input() result: ExecuteEtlResponse | null = null;

  @Output() runClicked = new EventEmitter<void>();

  protected get processedAndErrors(): number {
    if (!this.result) return 0;
    return this.result.processedCount + this.result.errorCount;
  }

  protected get errorRate(): number {
    if (!this.result || this.processedAndErrors === 0) return 0;
    return this.result.errorCount / this.processedAndErrors;
  }
}
