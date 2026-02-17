export interface SimulateInvoiceRequest {
  tariffId: string;
  kwhConsumption: number;
}

export interface InvoiceComponent {
  name: string;
  value: number;
  explanation: string;
}

export interface SimulateInvoiceResponse {
  tariffId: string;
  companyId: string;
  companyName: string;
  kwhConsumption: number;
  consumptionCost: number;
  transportCost: number;
  marketingCost: number;
  totalCost: number;
  components: InvoiceComponent[];
}
