import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToastMessage {
  id: number;
  title: string;
  message: string;
  type: 'success' | 'info' | 'warning' | 'danger';
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastsSource = new BehaviorSubject<ToastMessage[]>([]);
  toasts$ = this.toastsSource.asObservable();
  private nextId = 1;

  show(title: string, message: string, type: 'success' | 'info' | 'warning' | 'danger' = 'success', duration = 3500) {
    const id = this.nextId++;
    const toast: ToastMessage = { id, title, message, type };
    const currentToasts = this.toastsSource.value;
    this.toastsSource.next([...currentToasts, toast]);

    setTimeout(() => {
      this.remove(id);
    }, duration);
  }

  success(message: string, title = 'Success') {
    this.show(title, message, 'success');
  }

  error(message: string, title = 'Error') {
    this.show(title, message, 'danger');
  }

  info(message: string, title = 'Info') {
    this.show(title, message, 'info');
  }

  warning(message: string, title = 'Warning') {
    this.show(title, message, 'warning');
  }

  remove(id: number) {
    const updated = this.toastsSource.value.filter(t => t.id !== id);
    this.toastsSource.next(updated);
  }
}
