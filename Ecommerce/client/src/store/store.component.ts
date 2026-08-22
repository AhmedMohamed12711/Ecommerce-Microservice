import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { IBrand, IProduct, IType } from '../shared/models/product';
import { StoreParam } from '../shared/models/storeParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-store',
  standalone: false,
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss'
})
export class StoreComponent implements OnInit {
  @ViewChild('search') searchTerm?:ElementRef 
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  storeParams = new StoreParam();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price :Ascending', value: 'priceAsc' },
    { name: 'Price :Descending', value: 'priceDesc' }
  ];
  totalCount = 0;

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.getAllProducts();
    this.getAllBrands();
    this.getAllTypes();
  }

  getAllProducts() {
    this.storeService.getAllProducts(this.storeParams).subscribe({
      next: res => {
        this.products = res.data;
        this.totalCount = res.count;
        this.storeParams.PageIndex = res.pageIndex;
        this.storeParams.PageSize = res.pageSize;
      },
      error: error => {
        console.log(error);
      }
    });
  }

  getAllBrands() {
    this.storeService.getAllBrands().subscribe({
      next: res => {
        this.brands = [{ id: '', name: 'All' }, ...res];
      },
      error: error => {
        console.log(error);
      }
    });
  }

  getAllTypes() {
    this.storeService.getAllTypes().subscribe({
      next: res => {
        this.types = [{ id: '', name: 'All' }, ...res];
      },
      error: error => {
        console.log(error);
      }
    });
  }

  onBrandSelected(brandId: string) {
    this.storeParams.BrandId = brandId;
    this.storeParams.PageIndex = 1;
    this.getAllProducts();
  }

  onTypeSelected(typeId: string) {
    this.storeParams.TypeId = typeId;
    this.storeParams.PageIndex = 1;
    this.getAllProducts();
  }

  onSortSelected(sort: any) {
    this.storeParams.Sort = sort.value;
    this.getAllProducts();
  }

  onPageChanged(event: PageChangedEvent): void {
    if (this.storeParams.PageIndex !== event.page) {
      this.storeParams.PageIndex = event.page;
      this.getAllProducts();
    }
  }

  onPageSizeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    if (target) {
      this.storeParams.PageSize = Number(target.value);
      this.storeParams.PageIndex = 1;
      this.getAllProducts();
    }
  }

  onSearch(){
    this.storeParams.Search=this.searchTerm?.nativeElement.value
    this.storeParams.PageIndex=1
    this.getAllProducts()
  }

  onReset() {
    this.storeParams = new StoreParam();
    this.getAllProducts();
  }
}
