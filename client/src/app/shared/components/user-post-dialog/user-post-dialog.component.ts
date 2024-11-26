import { Component, Input } from '@angular/core';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { CommonModule } from '@angular/common';
import { TuiAvatar } from '@taiga-ui/kit';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word/first-letter-word.pipe';
import { TuiIcon } from '@taiga-ui/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-post-dialog',
  standalone: true,
  imports: [
    CommonModule,
    TuiAvatar,
    FirstLetterWordPipe,
    TuiIcon
  ],
  templateUrl: './user-post-dialog.component.html',
  styleUrl: './user-post-dialog.component.css'
})
export class UserPostDialogComponent {
  @Input() user: any;
  constructor(
    private router: Router
  ) { }

  ngOnInit() {
    console.log(this.user);
  }

  getUserProfile() {
    this.router.navigate([`/user/user-profile/${this.user?.userName}`]);
  }

}
