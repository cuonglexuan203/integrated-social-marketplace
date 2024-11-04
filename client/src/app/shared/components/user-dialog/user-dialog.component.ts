import { Component } from '@angular/core';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word.pipe';
import { TuiRating } from '@taiga-ui/kit';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TuiIcon } from '@taiga-ui/core';
import { UserDialogCardData } from './user-dialog.data';

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
  userDialogCartData = UserDialogCardData;
  value: number = 0;
  constructor() { }

  ngOnInit() { }
}
