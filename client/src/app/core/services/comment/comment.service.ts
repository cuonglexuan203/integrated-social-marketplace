import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';


@Injectable({
  providedIn: 'root'
})
export class CommentService {
  apiBase = environment.apiComment;
  constructor(
    private http: HttpClient
  ) { }

  createComment(commentData: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreateComment`, commentData);
  }

  getCommentsByPostId(postId: string): Observable<MarketplaceResponse<any>> {
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/GetCommentsByPostId/${postId}`);
  }

}
