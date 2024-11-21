import { Component, inject, INJECTOR } from '@angular/core';
import { TuiButton, TuiDialogService } from '@taiga-ui/core';
import { TuiAvatar } from '@taiga-ui/kit';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { CreatePostDialogComponent } from '../../../shared/components/create-post-dialog/create-post-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { Helper } from '../../../core/utils/helper';
import { NbAuthService } from '@nebular/auth';

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
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  user: UserResponseModel | null;
  constructor(
    private authService: NbAuthService,
  ) { }

  ngOnInit() {
    this.getUser();
  }

  getUser(){
    this.user = Helper.getUserFromLocalStorage();
    console.log(this.user);
    
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
        console.log(data);
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {

      }
    })
  }
}
