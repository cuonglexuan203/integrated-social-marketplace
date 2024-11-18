import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedPost } from '../../models/feed/feed.model';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';
import { CreatePostModel } from '../../models/feed/post.model';


@Injectable({
  providedIn: 'root'
})
export class FeedService {
  apiBase = environment.apiFeed;
  constructor(
    private http: HttpClient
  ) { }

  getAllPosts(): Observable<MarketplaceResponse<FeedPost[]>> {
    return this.http.get<MarketplaceResponse<FeedPost[]>>(`${this.apiBase}/GetAllPosts`);
  }

  createPost(post: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreatePost`, post);
  }

}
