import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/endpoint';
import { LoginModel } from '../../models/login/login.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiBase = environment.apiAuth;
  constructor(
    private http: HttpClient
  ) { }

  login(data: LoginModel): Observable<any> {
    return this.http.post(`${this.apiBase}/login`, data);
  }
}
