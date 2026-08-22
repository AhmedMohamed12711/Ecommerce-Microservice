import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IProduct } from '../../shared/models/product';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private wishlistKey = 'stepsshop_wishlist';
  private wishlistSource = new BehaviorSubject<IProduct[]>(this.loadWishlist());
  wishlist$ = this.wishlistSource.asObservable();

  constructor(private toastService: ToastService) {}

  private loadWishlist(): IProduct[] {
    const saved = localStorage.getItem(this.wishlistKey);
    return saved ? JSON.parse(saved) : [];
  }

  private saveWishlist(items: IProduct[]) {
    localStorage.setItem(this.wishlistKey, JSON.stringify(items));
    this.wishlistSource.next(items);
  }

  isInWishlist(productId: string): boolean {
    return this.wishlistSource.value.some(p => p.id === productId);
  }

  toggleWishlist(product: IProduct) {
    const current = this.wishlistSource.value;
    const exists = current.some(p => p.id === product.id);

    if (exists) {
      const updated = current.filter(p => p.id !== product.id);
      this.saveWishlist(updated);
      this.toastService.info(`Removed ${product.name} from Wishlist`, 'Wishlist Updated');
    } else {
      const updated = [...current, product];
      this.saveWishlist(updated);
      this.toastService.success(`Added ${product.name} to Wishlist!`, 'Wishlist Updated');
    }
  }

  removeItem(productId: string) {
    const current = this.wishlistSource.value;
    const target = current.find(p => p.id === productId);
    const updated = current.filter(p => p.id !== productId);
    this.saveWishlist(updated);
    if (target) {
      this.toastService.info(`Removed ${target.name} from Wishlist`, 'Wishlist Updated');
    }
  }

  clearWishlist() {
    this.saveWishlist([]);
    this.toastService.warning('Wishlist cleared', 'Wishlist Emptied');
  }

  getWishlistCount(): number {
    return this.wishlistSource.value.length;
  }
}
