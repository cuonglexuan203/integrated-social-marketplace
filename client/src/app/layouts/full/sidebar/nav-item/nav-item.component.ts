import { Component, EventEmitter, inject, INJECTOR, Input, Output } from '@angular/core';
import { NavItem } from './nav-item';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TuiIcon, TuiHint, TuiDialogService } from '@taiga-ui/core';
import { TuiTooltip } from '@taiga-ui/kit';
import { CreatePostDialogComponent } from '../../../../shared/components/create-post-dialog/create-post-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';

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

  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);
  constructor() { }

  ngOnInit() {
  }


  onClickNavItem(navItemName: string | undefined) {
    switch (navItemName) {
      case 'Create Post':
        this.createPost();
        break;
    }
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
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {

      }
    })
  }
}
