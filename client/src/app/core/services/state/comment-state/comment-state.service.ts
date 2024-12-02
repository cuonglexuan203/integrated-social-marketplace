import { Injectable } from '@angular/core';
import { Comment } from '../../../models/comment/comment.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentStateService {
  private commentsSubject = new BehaviorSubject<Comment[]>([]);
  comments$ = this.commentsSubject.asObservable();

  constructor() { }

  updateComments(comments: Comment[]) {
    this.commentsSubject.next(comments);
  }

  addComment(newComment: Comment) {
    const currentComments = this.commentsSubject.getValue();
    this.commentsSubject.next([...currentComments, newComment]);
  }
}
