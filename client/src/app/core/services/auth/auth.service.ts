import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/endpoint';
import { LoginModel } from '../../models/login/login.model';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';
import { RegisterModel } from '../../models/register/register.model';
import { UserSecurityModel } from '../../models/user/user-security.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiBase = environment.apiAuth;
  constructor(
    private http: HttpClient
  ) { }

  login(data: LoginModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/login`, data);
  }

  register(data: RegisterModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/register`, data);
  }

  changePassword(data: UserSecurityModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/ChangePassword`, data);
  }
}
