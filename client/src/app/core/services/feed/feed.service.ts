import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedPost } from '../../models/feed/feed.model';


@Injectable({
  providedIn: 'root'
})
export class FeedService {
  apiBase = environment.apiFeed;
  constructor(
    private http: HttpClient
  ) { }

  getFeed(): Observable<FeedPost> {
    return this.http.get<FeedPost>(`${this.apiBase}/feed`);
  }

}
