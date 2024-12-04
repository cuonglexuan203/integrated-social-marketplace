import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedPost } from '../../models/feed/feed.model';
import { MarketplaceResponse } from '../../models/marketplace/marketplace-response.model';
import { CreatePostModel } from '../../models/feed/post.model';
import { CommentRequestModel } from '../../models/comment/comment-request.model';
import { ReactionRequestModel } from '../../models/reaction/reaction.model';
import { Page, PageUserDetail } from '../../models/page/page.model';
import { ReportPostModel, UpdateValidityModel } from '../../models/feed/report.model';


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

  getPostByUserId(page: PageUserDetail): Observable<MarketplaceResponse<any>> { 
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/GetPosts?PageSize=${page.pageSize}&PageIndex=${page.pageIndex}&Sort=${page.sort}&UserId=${page.userId}`);
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

  createSavedPost(postId: string): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreateSavedPost`, {'postId': postId});
  }

  getSavedPostByUserId(userId: string): Observable<MarketplaceResponse<any>> {
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/GetAllUserSavedPosts/${userId}`);
  }

  unSavedPost(postId: string): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/UnSavePost`, {'postId': postId});
  }

  reportPost(report: ReportPostModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/CreateReport`, report);
  }

  getAllReports(page: Page): Observable<MarketplaceResponse<any>> {
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/GetReports?PageIndex=${page.pageIndex}`);
  }

  updateReportValidity(dataSending: UpdateValidityModel): Observable<MarketplaceResponse<any>> {
    return this.http.post<MarketplaceResponse<any>>(`${this.apiBase}/UpdateReportValidity`, dataSending);
  }

  searchPosts(searchValue: string): Observable<MarketplaceResponse<any>> {
    return this.http.get<MarketplaceResponse<any>>(`${this.apiBase}/SearchPosts?Search=${searchValue}`);
  }



}
