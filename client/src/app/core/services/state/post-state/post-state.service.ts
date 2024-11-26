import { Injectable } from '@angular/core';
import { FeedPost } from '../../../models/feed/feed.model';

@Injectable({
  providedIn: 'root'
})
export class PostStateService {
  savedPosts: FeedPost[] = []
  constructor() { }

  getSavedPosts() {
    return this.savedPosts
  }

  setSavedPosts(savedPosts: FeedPost[]) {
    this.savedPosts = savedPosts
  }
}
