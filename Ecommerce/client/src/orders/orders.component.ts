import { Component, OnInit } from '@angular/core';
import { IOrder } from './order.model';
import { OrdersService } from './orders.service';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-orders',
  standalone: false,
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  orders: IOrder[] = [];
  loading = true;

  constructor(private ordersService: OrdersService, private accountService: AccountService) {}

  ngOnInit(): void {
    const userName = this.accountService.getUserName();
    const altUserName = userName === 'alice' ? 'ahmed' : 'alice';

    this.ordersService.getOrdersForUser(userName).subscribe({
      next: (primaryOrders) => {
        this.ordersService.getOrdersForUser(altUserName).subscribe({
          next: (altOrders) => {
            const combined = [...(primaryOrders || []), ...(altOrders || [])];
            // Remove duplicates by order id
            const uniqueMap = new Map<number, IOrder>();
            combined.forEach(o => uniqueMap.set(o.id, o));
            this.orders = Array.from(uniqueMap.values());
            this.loading = false;
          },
          error: () => {
            this.orders = primaryOrders || [];
            this.loading = false;
          }
        });
      },
      error: (err) => {
        console.error('Error fetching primary orders:', err);
        this.orders = [];
        this.loading = false;
      }
    });
  }
}
