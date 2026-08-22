import { Component } from '@angular/core';
import { ToastService, ToastMessage } from '../../services/toast.service';

@Component({
  selector: 'app-toast-container',
  standalone: false,
  templateUrl: './toast-container.component.html',
  styleUrl: './toast-container.component.scss'
})
export class ToastContainerComponent {
  constructor(public toastService: ToastService) {}

  remove(id: number) {
    this.toastService.remove(id);
  }

  getIconClass(type: string): string {
    switch (type) {
      case 'success': return 'fa-check-circle text-success';
      case 'danger': return 'fa-exclamation-circle text-danger';
      case 'warning': return 'fa-exclamation-triangle text-warning';
      default: return 'fa-info-circle text-info';
    }
  }
}
