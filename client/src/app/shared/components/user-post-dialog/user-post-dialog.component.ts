import { Component, Input } from '@angular/core';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { CommonModule } from '@angular/common';
import { TuiAvatar } from '@taiga-ui/kit';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word/first-letter-word.pipe';
import { TuiDropdown, TuiIcon, TuiLoader } from '@taiga-ui/core';
import { Router } from '@angular/router';
import { Helper } from '../../../core/utils/helper';
import { UserService } from '../../../core/services/user/user.service';
import { UserFollowModel } from '../../../core/models/user/user-dialog.model';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
  selector: 'app-user-post-dialog',
  standalone: true,
  imports: [
    CommonModule,
    TuiAvatar,
    FirstLetterWordPipe,
    TuiIcon,
  ],
  templateUrl: './user-post-dialog.component.html',
  styleUrl: './user-post-dialog.component.css'
})
export class UserPostDialogComponent {
  isLoading: boolean = false;

  userLoggedIn: UserResponseModel;
  @Input() user: any;
  @Input() isFromFollow: boolean;

  isCheckedUserLoggedIn: boolean = false;

  constructor(
    private router: Router,
    private _userService: UserService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.userLoggedIn = Helper.getUserFromLocalStorage();
    this.checkUserLoggedIn();
  }

  checkUserLoggedIn() {
    if (this.userLoggedIn && this.user) {
      if (this.userLoggedIn.id === this.user.id) {
        this.isCheckedUserLoggedIn = true;
      }
    }
  }

  getUserProfile() {
    this.router.navigate([`/user/user-profile/${this.user?.userName}`]);
  }

  unFollowAction() {
    const dataSending: UserFollowModel = {
      followerId: this.userLoggedIn.id,
      followedId: this.user.id
    }

    this.isLoading = true;
    this._userService.unFollowUser(dataSending).subscribe({
      next: (res) => {
        if (res?.result) {
          this.user.isFollowing = false;
          this.alertService.showSuccess('UnFollowed successfully', "Success");
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
        this.alertService.showError('Failed to UnFollowed', "Error");
      },
      complete: () => {
        // this.checkIsUserFollowing();
        this.isLoading = false;
      },
    })
  }

  followAction() {
    const dataSending: UserFollowModel = {
      followerId: this.userLoggedIn.id,
      followedId: this.user.id
    }

    this.isLoading = true;
    this._userService.followUser(dataSending).subscribe({
      next: (res) => {
        if (res?.result) {
          this.user.isFollowing = true;
          this.alertService.showSuccess('Followed successfully', "Success");
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
        this.alertService.showError('Failed to follow', "Error");
      },
      complete: () => {
        // this.checkIsUserFollowing();
        this.isLoading = false;
      },
    })
  }

  messageAction() {

  }

}
