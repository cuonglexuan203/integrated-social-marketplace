import { Component, EventEmitter, inject, INJECTOR, Output } from '@angular/core';
import { TuiButton, TuiDialogService } from '@taiga-ui/core';
import { TuiAvatar } from '@taiga-ui/kit';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { CreatePostDialogComponent } from '../../../shared/components/create-post-dialog/create-post-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { Helper } from '../../../core/utils/helper';
import { NbAuthService } from '@nebular/auth';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { UserService } from '../../../core/services/user/user.service';
import { AlertService } from '../../../core/services/alert/alert.service';

@Component({
  selector: 'app-user-new-post',
  standalone: true,
  imports: [
    TuiAvatar,
    TuiButton,

  ],
  templateUrl: './user-new-post.component.html',
  styleUrl: './user-new-post.component.css'
})
export class UserNewPostComponent {
  @Output() postsCreated = new EventEmitter<any>();
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  userId: string;
  user: UserResponseModel | null;
  constructor(
    private authService: NbAuthService,
    private _userService: UserService,
    private alertService: AlertService,
  ) {
   }

  ngOnInit() {
    this.getUserToken();
    this.getUserDetail();
  }

  async getUserToken() {
    this.authService.onTokenChange().subscribe((token) => {
      if (token?.isValid()) {
        this.userId = token?.getPayload()?.userId;
      }
    });
  }

  async getUserDetail() {
    this._userService.getUserDetail(this.userId).subscribe({
      next: (res) => {
        if (res) {
          this.user = res.result;
          localStorage.setItem('user', JSON.stringify(this.user));
        }
      },
      error: (error) => {
        this.alertService.showError('Error', error);
      },
      complete: () => {

      }
    })
  }

  

  onFocus() {
   this.createPost(); 
  }

  createPost() {
    const data = { title: 'Create Post' };
    this.dialogs.open(
      new PolymorpheusComponent(CreatePostDialogComponent, this.injector),
      {
        data: data,
        dismissible: false,
      }
    ).subscribe({
      next: (data) => {
        this.postsCreated.emit(data);        
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {

      }
    })
  }
}
