import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { BasketService } from '../../basket/basket.service';
import { WishlistService } from '../../core/services/wishlist.service';

@Component({
  selector: 'app-product-items',
  standalone: false,
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss'
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor(private basketService: BasketService, public wishlistService: WishlistService){}
  
  onImgError(event: Event) {
    const target = event.target as HTMLImageElement;
    if (target && !target.src.includes('placeholder.png')) {
      target.src = 'images/placeholder.png';
    }
  }

  addItemBasket(){
    this.product && this.basketService.addItemtoBasket(this.product)
  }

  toggleWishlist(event: Event) {
    event.stopPropagation();
    if (this.product) {
      this.wishlistService.toggleWishlist(this.product);
    }
  }

  isWishlisted(): boolean {
    return this.product ? this.wishlistService.isInWishlist(this.product.id) : false;
  }
}
