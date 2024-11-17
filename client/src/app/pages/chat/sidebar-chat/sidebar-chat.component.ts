import { Component } from '@angular/core';
import { SidebarChatHeaderData } from '../../../core/data/sidebar-chat-header-data';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { TuiAvatar, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-sidebar-chat',
  standalone: true,
  imports: [
    TuiIcon,
    TuiTextfield,
    TuiSearch,
    TuiAvatar,
    TuiBadgedContent,
    TuiCell,
    TuiIcon,
    CommonModule,
    TuiSwitch,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './sidebar-chat.component.html',
  styleUrl: './sidebar-chat.component.css'
})
export class SidebarChatComponent {
  sidebarChatHeaderData = SidebarChatHeaderData;
  userMockData: any[] = []
  selectedUserChat: any;
  unRead = false;
  constructor() { }
  ngOnInit() {
    for(let i =0 ; i <= 10; i++) {
      this.userMockData.push(
        {
          id: this.generateRandomId(),
          userName: 'John Doe',
          userImg: 'assets/images/user-1.jpg',
          lastMessage: 'Hey, how are you?',
        },
        {
          id: this.generateRandomId(),
          userName: 'John Doe',
          userImg: 'assets/images/user-1.jpg',
          lastMessage: 'Hey, how are you?',
        }
      )
    }
    this.selectedUserChat = this.userMockData[0];
  }

  handleClickUserChat(user: any) {
    this.selectedUserChat = user;
  }

  generateRandomId(): number {
    return Math.floor(Math.random() * 1000000); // Generates a random ID between 0 and 999999
  }
}
