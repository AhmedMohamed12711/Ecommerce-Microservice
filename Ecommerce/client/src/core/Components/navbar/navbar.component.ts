import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../../basket/basket.service';
import { IBasketItem } from '../../../shared/models/basket';
import { AccountService } from '../../../account/account.service';
import { WishlistService } from '../../../core/services/wishlist.service';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit{
  isCollapsed = true;
  isUserAuthenticated:boolean=false
  constructor(public basketservice:BasketService, public accountService:AccountService, public wishlistService:WishlistService ){}
  ngOnInit(): void {
    this.accountService.currentUser$.subscribe({
      next:(res)=>{
        this.isUserAuthenticated=res
        console.log('is authenticated'+this.isUserAuthenticated);
        
      },
      error:(err)=>{
        console.log('An error occured while setting authentication flag ');
        
      }
    })
  }

  login(){
    this.accountService.login()
  }
  logout(){
    this.accountService.logout()
  }
  getBasketCount(items:IBasketItem[]):number{
    return items.reduce((sum,item)=>sum+item.quantity,0)
  }
}
