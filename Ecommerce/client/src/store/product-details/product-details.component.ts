import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from '../../basket/basket.service';
import { WishlistService } from '../../core/services/wishlist.service';

@Component({
  selector: 'app-product-details',
  standalone: false,
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit{
  product?:IProduct
  quantity=1;

  
  constructor(
    private storeService:StoreService,
    private activatedRoute:ActivatedRoute,
    private breadService:BreadcrumbService,
    private basketService:BasketService,
    public wishlistService:WishlistService,
  ){}

  toggleWishlist() {
    if (this.product) {
      this.wishlistService.toggleWishlist(this.product);
    }
  }

  isWishlisted(): boolean {
    return this.product ? this.wishlistService.isInWishlist(this.product.id) : false;
  }

  
  



  dec() { if (this.quantity > 1) this.quantity--; }
  inc() { this.quantity++; }

  addToCart() {
    if (!this.product) return;
    this.basketService.addItemtoBasket(this.product, this.quantity);
  }

  ngOnInit(): void {
    this.loadProduct();
  }
  loadProduct(){
    const id =this.activatedRoute.snapshot.paramMap.get('id')

    this.storeService.getProductById(id!).subscribe(
      {
        next:res=>{
          this.product=res
          this.breadService.set('@productDetails',res.name)
          console.log(res);
          
        },
        error:err=>{
          console.log(err)
        }
      }
    )
  }
}
