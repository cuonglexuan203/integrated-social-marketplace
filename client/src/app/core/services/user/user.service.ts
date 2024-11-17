import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/endpoint';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiBase = environment.apiUser
  constructor(
    private http: HttpClient,
  ) { }

  getUserDetail(userId: string): Observable<any> {
    return this.http.get(`${this.apiBase}/GetUserDetails/${userId}`);
  }
}
