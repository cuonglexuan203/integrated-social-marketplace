import { Component } from '@angular/core';
import { SidebarChatComponent } from './sidebar-chat/sidebar-chat.component';
import { ChatboxComponent } from './chatbox/chatbox.component';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    SidebarChatComponent,
    ChatboxComponent
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {

}
