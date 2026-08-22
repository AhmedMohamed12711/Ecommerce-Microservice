import { Component, OnInit } from '@angular/core';
import { WishlistService } from '../core/services/wishlist.service';
import { BasketService } from '../basket/basket.service';
import { IProduct } from '../shared/models/product';

@Component({
  selector: 'app-wishlist',
  standalone: false,
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.scss'
})
export class WishlistComponent implements OnInit {
  constructor(public wishlistService: WishlistService, private basketService: BasketService) {}

  ngOnInit(): void {}

  addToCart(product: IProduct) {
    this.basketService.addItemtoBasket(product);
  }

  removeFromWishlist(product: IProduct) {
    this.wishlistService.toggleWishlist(product);
  }

  clearAll() {
    this.wishlistService.clearWishlist();
  }
}
