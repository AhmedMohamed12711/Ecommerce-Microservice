import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IResponseDto } from '../shared/models/response';
import { IBrand, IProduct, IType } from '../shared/models/product';
import { StoreParam } from '../shared/models/storeParams';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  baseUrl:string="https://forty-islands-flow.loca.lt/";
  constructor(private http: HttpClient) {}

  getAllProducts(storeParam: StoreParam) {
    let params = new HttpParams();
    if (storeParam.BrandId) {
      params = params.append('brandId', storeParam.BrandId);
    }
    if (storeParam.TypeId) {
      params = params.append('typeId', storeParam.TypeId);
    }
    if (storeParam.PageIndex) {
      params = params.append('pageIndex', storeParam.PageIndex.toString());
    }
    if (storeParam.PageSize) {
      params = params.append('pageSize', storeParam.PageSize.toString());
    }
    if (storeParam.Sort) {
      params = params.append('sort', storeParam.Sort);
    }
    if (storeParam.Search) {
      params = params.append('search', storeParam.Search);
    }
    return this.http.get<IResponseDto<IProduct[]>>(
      `${this.baseUrl}Catalog/GetAllProducts`, { params }
    );
  }

  getAllBrands() {
    return this.http.get<IBrand[]>(
      `${this.baseUrl}Catalog/GetAllBrands`
    );
  }

  getAllTypes() {
    return this.http.get<IType[]>(
      `${this.baseUrl}Catalog/GetAllTypes`
    );
  }
  getProductById(id:string) {
    return this.http.get<IProduct>(
      `${this.baseUrl}Catalog/GetProductById/${id}`
    );
  }
}
