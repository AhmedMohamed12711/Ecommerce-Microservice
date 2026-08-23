import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';
import { AccountService } from '../account/account.service';
import { Router } from '@angular/router';
import { ToastService } from '../core/services/toast.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl:string="http://localhost:8010/"
  private basketSource=new BehaviorSubject<Basket|null>(null)
  basketSource$=this.basketSource.asObservable()

  private basketTotal=new BehaviorSubject<IBasketTotal|null>(null)
  basketTotal$=this.basketTotal.asObservable()
  constructor(private http:HttpClient, private accountService:AccountService, private router:Router, private toastService: ToastService) { }

  getBasket(userName:string){
    return this.http.get<IBasket>(this.baseUrl+'Basket/GetBasket/'+userName).subscribe({
      next:basket=>{
        this.basketSource.next(basket)
        this.calculateBasketTotal()
      }
    })
  }

  setBasket(basket:IBasket){
    return this.http.post<IBasket>(this.baseUrl+'Basket/CreateBasket',basket).subscribe({
      next:basket=>{
        this.basketSource.next(basket)
        this.calculateBasketTotal()
      }
    })
  }

  getCurrentBasket(){
    return this.basketSource.value
  }

  addItemtoBasket(item:IProduct,quantity=1){
    const itemToAdd:IBasketItem=this.mapProductToBasketItem(item)
    const basket=this.getCurrentBasket()??this.createBasket()
    basket.items=this.addOrUpdateItem(basket.items,itemToAdd,quantity)
    this.setBasket(basket)
    this.toastService.success(`Added ${item.name} to cart!`, 'Cart Updated');
  }

  mapProductToBasketItem(item:IProduct):IBasketItem{
      return {
        productId:item.id,
        imageFile:item.imageFile,
        productName:item.name,
        price:item.price,
        quantity:0
        
      }
  }
  createBasket(){
      const basket = new Basket();
      basket.userName = this.accountService.getUserName();
      localStorage.setItem("basket_userName", basket.userName);
      return basket;
  }

  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const item=items.find(x=>x.productId==itemToAdd?.productId)
    if(item){
      item.quantity+=quantity
    }
    else{
      itemToAdd.quantity=quantity
      items.push(itemToAdd)
    }
    return items
  }

  
  incrementItemQuantity(item:IBasketItem){
    const basket=this.getCurrentBasket()
    if(!basket)return
    const foundItemIndex=basket.items.findIndex((x)=>x.productId===item.productId)
    basket.items[foundItemIndex].quantity++
    this.setBasket(basket)
  }
  removeItemFromBasket(item:IBasketItem){
    const basket=this.getCurrentBasket()
    if(!basket)return
    if(basket.items.some((x)=>x.productId===item.productId)){
      basket.items=basket.items.filter((x)=>x.productId!==item.productId)
      this.toastService.info(`Removed ${item.productName} from cart`, 'Cart Updated');
      if(basket.items.length>0){
        this.setBasket(basket)
      }
      else{
        this.deleteBasket(basket.userName)
      }
    }
  }
  deleteBasket(userName: string) {
    return this.http.delete(this.baseUrl+'Basket/DeleteBasket/'+userName).subscribe({
      next:res=>{
        this.basketSource.next(null)
        this.basketTotal.next(null)
        localStorage.removeItem("basket_userName")
      },
      error:err=>{
        console.log(err);
        
      }
    })
  }

  decrementItemQuantity(item:IBasketItem){
    const basket=this.getCurrentBasket()
    if(!basket)return
    const foundItemIndex=basket.items.findIndex((x)=>x.productId===item.productId)
    if(basket.items[foundItemIndex].quantity>1){

      basket.items[foundItemIndex].quantity--
      this.setBasket(basket)
    }else{
      this.removeItemFromBasket(item)
    }
  }

  checkoutBasket(basket:IBasket){
    basket.userName = this.accountService.getUserName();
    const httpOptions={
      headers:new HttpHeaders({
        'Content-Type':'application/json',
        'Authorization':this.accountService.authorizationHeaderValue
      })
    }
    return this.http.post<IBasket>(this.baseUrl+'Basket/CheckoutV2',basket,httpOptions).subscribe({
      next:res=>{
        this.basketSource.next(null)
        this.basketTotal.next(null)
        localStorage.removeItem("basket_userName")
        this.toastService.success('Order placed successfully! Thank you for shopping with us.', 'Order Completed');
        this.router.navigateByUrl('/')
      },
      error:err=>{
        this.toastService.error('Failed to process checkout. Please try again.', 'Checkout Error');
      }
    })
  }

  private calculateBasketTotal(){
    const basket=this.getCurrentBasket()
    if(!basket)return
    const total=basket.items.reduce((x,y)=>(y.price*y.quantity)+x,0)
    basket.totalPrice = total;
    this.basketTotal.next({total})
  }
}

