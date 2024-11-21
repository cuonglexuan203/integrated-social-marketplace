import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/endpoint';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';
import { UserResponseModel } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiBase = environment.apiUser
  constructor(
    private http: HttpClient,
  ) { }

  getUserDetail(userId: string): Observable<MarketplaceResponse<UserResponseModel>> {
    return this.http.get<MarketplaceResponse<UserResponseModel>>(`${this.apiBase}/GetUserDetails/${userId}`);
  }
}
