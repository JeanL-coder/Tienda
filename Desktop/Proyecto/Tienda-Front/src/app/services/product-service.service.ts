import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Producto } from '../models/Producto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrl = 'https://localhost:7007/api/Product/mostsearched';

  constructor(private http: HttpClient) { }

  getMostSearchedProducts(): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${this.baseUrl}/most-searched`);
  }
}
