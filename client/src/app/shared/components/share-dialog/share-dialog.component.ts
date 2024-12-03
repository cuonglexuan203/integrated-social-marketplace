import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiDialogContext, TuiIcon } from '@taiga-ui/core';
import { TuiRadioList } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { FeedService } from '../../../core/services/feed/feed.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { CreatePostModel } from '../../../core/models/feed/post.model';
import { AlertService } from '../../../core/services/alert/alert.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-share-dialog',
  standalone: true,
  imports: [
    TuiRadioList,
    FormsModule,
    ReactiveFormsModule,
    TuiButton,
    TuiIcon,
    CommonModule
  ],
  templateUrl: './share-dialog.component.html',
  styleUrls: ['./share-dialog.component.css']
})
export class ShareDialogComponent {
  public readonly context = injectContext<TuiDialogContext<any>>();

  isLoading: boolean = false;
  data: any;
  title: string = '';
  listSelection: string[] = [];
  selectedItem: string | null = null; // Variable to track selected radio item
  post: FeedPost;
  newPostShareContent: string = '';
  user: UserResponseModel;
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService

  ) {
    this.data = this.context.data;
  }



  ngOnInit() {
    this.title = this.data.title;
    this.listSelection = this.data.listSelection;
    this.selectedItem = this.listSelection[0];
    this.post = this.data.post;
    this.user = Helper.getUserFromLocalStorage();
  }

  onSelectionChange() {
    if (this.selectedItem) {
      if (this.selectedItem === 'On my wall') {
      }
    }
  }

  sharePost() {
    if (this.selectedItem === 'On my wall') {
      const sendingData = {
        userId: this.user.id,
        contentText: this.newPostShareContent,
        sharedPostId: this.post.id,
      }
      this.isLoading = true;
      this._feedService.createPost({ sendingData }).subscribe({
        next: (res) => {
          if (res?.result) {
            this.context.completeWith(res.result);
            this.alertService.showSuccess('Post shared successfully', 'Success');
            this.isLoading = false;
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.alertService.showError('Cannot share post', 'Error');
          console.log(err)
        },
        complete: () => {
          this.isLoading = false;
        }
      })
    }
  }
}
