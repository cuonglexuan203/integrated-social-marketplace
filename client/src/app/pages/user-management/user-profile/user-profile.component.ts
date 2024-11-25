import { Component, Output } from '@angular/core';
import { UserNewPostComponent } from '../../home/user-new-post/user-new-post.component';
import { TuiAvatar } from '@taiga-ui/kit';
import { TuiIcon } from '@taiga-ui/core';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { PostItemComponent } from '../../home/post-item/post-item.component';
import { AlertService } from '../../../core/services/alert/alert.service';
import { FeedService } from '../../../core/services/feed/feed.service';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { Page } from '../../../core/models/page/page.model';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    UserNewPostComponent,
    TuiAvatar,
    TuiIcon,
    PostItemComponent,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  @Output() posts: FeedPost[] = [];
  user: UserResponseModel;
  page: Page
  constructor(
    private alertService: AlertService,
    private _feedService: FeedService,
  ) { }

  ngOnInit() {
    this.getUser();
    this.getPosts();
  }

  getUser() {
    this.user = Helper.getUserFromLocalStorage();

  }

  getPosts() {
    this.page = {
      pageIndex: 1,
      pageSize: 10,
      sort: 'desc'
    }
    this._feedService.getPosts(this.page).subscribe({
      next: (res) => {
        if (res) {
          this.posts = res?.result?.data;
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }


}
