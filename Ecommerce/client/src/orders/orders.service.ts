import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { IOrder } from './order.model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = 'https://every-birds-fix.loca.lt/';

  constructor(private http: HttpClient) { }

  getOrdersForUser(userName: string): Observable<IOrder[]> {
    return this.http.get<IOrder[]>(this.baseUrl + 'Order/' + userName).pipe(
      catchError(err => {
        console.warn('Orders API returned error or no orders found:', err);
        return of([]);
      })
    );
  }
}
