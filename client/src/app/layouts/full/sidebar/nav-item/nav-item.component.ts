import { Component, EventEmitter, inject, INJECTOR, Input, Output } from '@angular/core';
import { NavItem } from './nav-item';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TuiIcon, TuiHint, TuiDialogService } from '@taiga-ui/core';
import { TuiTooltip } from '@taiga-ui/kit';
import { CreatePostDialogComponent } from '../../../../shared/components/create-post-dialog/create-post-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { UserResponseModel } from '../../../../core/models/user/user.model';
import { Helper } from '../../../../core/utils/helper';

@Component({
  selector: 'app-nav-item',
  standalone: true,
  imports: [CommonModule,
    RouterModule,
    TuiIcon,
    TuiTooltip,
    TuiHint
  ],
  templateUrl: './nav-item.component.html',
  styleUrl: './nav-item.component.css'
})
export class NavItemComponent {
  @Input() item: NavItem;
  @Input() isToggle: boolean;
  user: UserResponseModel;

  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);
  constructor(
    private router: Router,
  ) { }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.user = Helper.getUserFromLocalStorage();
  }

  onClickNavItem(navItemName: string | undefined) {
    
    switch (navItemName) {
      case 'Chat':
        this.router.navigate(['/chat']);
        break;
      case 'User Profile':
        this.router.navigate([`/user/user-profile/${this.user?.userName}`]);
        break;
      case 'Home':
        this.router.navigate(['/home']);
        break;
      case 'Saved Posts':
        this.router.navigate(['/user/saved-post']);
        break;
    }
  }

  // createPost() {
  //   const data = { title: 'Create Post' };
  //   this.dialogs.open(
  //     new PolymorpheusComponent(CreatePostDialogComponent, this.injector),
  //     {
  //       data: data,
  //       dismissible: false,
  //     }
  //   ).subscribe({
  //     next: (data) => {
  //     },
  //     error: (error) => {
  //       console.error(error);
  //     },
  //     complete: () => {

  //     }
  //   })
  // }
}
