import { Component, Input } from '@angular/core';
import { MoreDialogData, MoreDialogStateFilter } from '../../../core/data/more-dialog-data';
import { MoreDialogModel } from '../../../core/models/more/more-dialog.model';
import { FeedService } from '../../../core/services/feed/feed.service';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { AlertService } from '../../../core/services/alert/alert.service';
import { UserStateService } from '../../../core/services/state/user-state/user-state.service';
import { PostStateService } from '../../../core/services/state/post-state/post-state.service';

@Component({
  selector: 'app-more-dialog',
  standalone: true,
  imports: [],
  templateUrl: './more-dialog.component.html',
  styleUrl: './more-dialog.component.css'
})
export class MoreDialogComponent {
  @Input() post: FeedPost;
  moreDialogData = MoreDialogData;
  moreDialogStateFilter = MoreDialogStateFilter
  stateFilter: string;

  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
    private _userStateService: UserStateService,
    private _postStateService: PostStateService
  ) { }

  ngOnInit() {
    this.getUserStateFilter()
  }

  getUserStateFilter() {
    this.stateFilter = this._userStateService.getStateFilter() || ''
  }

  ngOnDestroy() {
    this.stateFilter = ''
  }

  onClickItemMoreDialog(item: MoreDialogModel) {
    if (item) {
      switch (item.action) {
        case 'addToBookmarks':
          this.addBookmark();
          break;
        case 'report':
          console.log('Report');
          break;
        case 'notInteresting':
          break;
        case 'removeSavedPost':
          this.removeSavedPost();
          break;
      }
    }
  }

  addBookmark() {
    this._feedService.createSavedPost(this.post.id).subscribe({
      next: (res) => {
        if (res?.result) {
          this.alertService.showSuccess('Post added to bookmarks', 'Success');
        }
      },
      error: (err: any) => {
        console.log(err);
        this.alertService.showError(err?.error?.detail, 'Error');
      }

    });
  }

  handleAfterRemovingSavedPost() {
    const savedPosts = this._postStateService.getSavedPosts();
    const index = savedPosts.findIndex((post) => post.id === this.post.id);
    if (index > -1) {
      savedPosts.splice(index, 1);
      this._postStateService.setSavedPosts(savedPosts);
    }
  }

  removeSavedPost() {
    this._feedService.unSavedPost(this.post.id).subscribe({
      next: (res) => {
        if (res?.result) {
          this.handleAfterRemovingSavedPost();
          this.alertService.showSuccess('Post removed from bookmarks', 'Success');
        }
      },
      error: (err: any) => {
        console.log(err);
        this.alertService.showError(err?.error?.detail, 'Error');
      }
    })
  }
}
