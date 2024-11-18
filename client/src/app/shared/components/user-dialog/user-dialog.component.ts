import { Component, Input } from '@angular/core';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word/first-letter-word.pipe';
import { TuiRating } from '@taiga-ui/kit';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TuiIcon } from '@taiga-ui/core';
import { UserConvenientData, UserDialogCardData, UserDialogMoreData } from './user-dialog.data';
import { Router } from '@angular/router';
import { UserResponseModel } from '../../../core/models/user/user.model';

@Component({
  selector: 'app-user-dialog',
  standalone: true,
  imports: [
    FirstLetterWordPipe,
    TuiRating,
    CommonModule,
    FormsModule,
    TuiIcon
  ],
  templateUrl: './user-dialog.component.html',
  styleUrl: './user-dialog.component.css'
})
export class UserDialogComponent {
  @Input() user: UserResponseModel;
  userDialogCartData = UserDialogCardData;
  userConvenientData = UserConvenientData;
  userDialogMoreData = UserDialogMoreData;
  value: number = 4;
  constructor(
    private router: Router,
  ) { }

  ngOnInit() { }

  handleClickMore(item: any) {
    if (item) {
      switch (item.iconName) { 
        case '@tui.settings':
          this.router.navigate(['/user/account-settings']);
          break;
        case '@tui.badge-help':
          break;
        case '@tui.lightbulb':
          break
      }
    }
  }
}
