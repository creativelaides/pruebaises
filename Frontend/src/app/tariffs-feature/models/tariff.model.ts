export interface TariffSummary {
  id: string;
  year: number;
  period: string;
  level: string;
  tariffOperator: string;
  companyId: string;
  totalCosts: number;
  createdAt: string;
}

export interface TariffListResponse { tariffs: TariffSummary[]; }
export interface TariffByPeriodResponse { tariffs: TariffSummary[]; }

export interface CreateTariffRequest {
  year: number;
  period: string;
  level: string;
  tariffOperator: string;
  companyId: string;
  totalCu: number | null;
  purchaseCostG: number | null;
  chargeTransportStnTm: number | null;
  chargeTransportSdlDm: number | null;
  marketingMargin: number | null;
  costLossesPr: number | null;
  restrictionsRm: number | null;
  cot: number | null;
  cfmjGfact: number | null;
}

export interface UpdateTariffRequest {
  totalCu: number | null;
  purchaseCostG: number | null;
  chargeTransportStnTm: number | null;
  chargeTransportSdlDm: number | null;
  marketingMargin: number | null;
  costLossesPr: number | null;
  restrictionsRm: number | null;
  cot: number | null;
  cfmjGfact: number | null;
}
