import { Injectable } from '@angular/core';
import { Comment } from '../../../models/comment/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentStateService {
   constructor(
   ) { }

  private comments: Comment[] = [];

   getComments(): Comment[] {
    return this.comments;
   }

    setComments(comments: Comment[]): void {
      this.comments = comments;
    }
  
}
