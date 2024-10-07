import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/endpoint';
import { MarketplaceResponse } from '../../core/models/marketplace/marketplace-response.model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiBase = environment.apiAuth;
  constructor(
    private http: HttpClient
  ) { }

  login(data: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(this.apiBase, data);
  }
}
