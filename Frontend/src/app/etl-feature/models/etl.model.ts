export interface ExecuteEtlResponse {
  success: boolean;
  processedCount: number;
  errorCount: number;
  message: string;
  durationSeconds: number;
  executionDate: string;
}
