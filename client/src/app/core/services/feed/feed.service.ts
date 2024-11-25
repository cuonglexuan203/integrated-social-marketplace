import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedPost } from '../../models/feed/feed.model';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';
import { CreatePostModel } from '../../models/feed/post.model';
import { CommentRequestModel } from '../../models/comment/comment-request.model';
import { ReactionRequestModel } from '../../models/reaction/reaction.model';
import { Page } from '../../models/page/page.model';


@Injectable({
  providedIn: 'root'
})
export class FeedService {
  apiBase = environment.apiFeed;
  constructor(
    private http: HttpClient
  ) { }

  // getAllPosts(): Observable<MarketplaceResponse<FeedPost[]>> {
  //   return this.http.get<MarketplaceResponse<FeedPost[]>>(`${this.apiBase}/GetAllPosts`);
  // }

  getPosts(page: Page): Observable<MarketplaceResponse<any>> {
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/GetPosts?PageSize${page.pageSize}&PageIndex=${page.pageIndex}&Sort=${page.sort}`);
  }

  createPost(post: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreatePost`, post);
  }

  createComment(commentData: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreateComment`, commentData);
  }

  addReaction(reactionData: ReactionRequestModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/AddReaction`, reactionData);
  }

  getReactionsByPostId(postId: string): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/GetReactionsByPostId/${postId}`, null);
  }

  removeReactionFromPost(dataSending: any): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/RemoveReactionFromPost`, dataSending);
  }

}
