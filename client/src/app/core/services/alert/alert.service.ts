import { Injectable } from '@angular/core';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  constructor(private alerts: TuiAlertService) {}

  showSuccess(message: string, label: string): void {
    this.alerts.open(message, {
      label: label,
      appearance: 'success',
      autoClose: 3000
    }).subscribe();
  }

  showWarning(message: string, label: string): void {
    this.alerts.open(message, {
      label: label,
      appearance: 'warning',
      autoClose: 3000
    }).subscribe();
  }

  showError(message: string, label: string): void {
    this.alerts.open(message, {
      label: label,
      appearance: 'error',
      autoClose: 3000
    }).subscribe();
  }

}