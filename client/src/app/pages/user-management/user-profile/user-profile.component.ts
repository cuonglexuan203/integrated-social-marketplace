import { Component, Output } from '@angular/core';
import { UserNewPostComponent } from '../../home/user-new-post/user-new-post.component';
import { TuiAvatar, TuiSkeleton } from '@taiga-ui/kit';
import { TuiIcon } from '@taiga-ui/core';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { PostItemComponent } from '../../home/post-item/post-item.component';
import { AlertService } from '../../../core/services/alert/alert.service';
import { FeedService } from '../../../core/services/feed/feed.service';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { Page } from '../../../core/models/page/page.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { UserService } from '../../../core/services/user/user.service';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    UserNewPostComponent,
    TuiAvatar,
    TuiIcon,
    PostItemComponent,
    CommonModule,
    TuiSkeleton,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  @Output() posts: FeedPost[] = [];
  user: UserResponseModel;
  page: Page

  isLoading: boolean = false;
  constructor(
    private alertService: AlertService,
    private _feedService: FeedService,
    private activatedRoute: ActivatedRoute,
    private _userService: UserService
  ) { }

  ngOnInit() {
    this.getUserFromUserName();
    this.getPosts();
  }

  getUserFromUserName() {
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      const userName = params.get('userName') || '';
      if (userName) {
        this.isLoading = true;
        this._userService.getUserDetailByUserName(userName).subscribe({
          next: (res) => {
            if (res) {
              this.user = res.result;
              this.isLoading = false
            }
            else {
              this.isLoading = false; 
            }
          },
          error: (error) => {
            console.error(error);
            this.isLoading = false;
          },
          complete: () => {
            this.isLoading = false;
          }

        });
      }
    })
  }

  getPosts() {
    this.page = {
      pageIndex: 1,
      pageSize: 10,
      sort: 'asc'
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
