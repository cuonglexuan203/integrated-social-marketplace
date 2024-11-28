import { CommonModule } from '@angular/common';
import { Component, Input, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiAvatar, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { ChatBoxHeaderData } from '../../../core/data/chatbox-header-data';
import { TuiInputModule } from '@taiga-ui/legacy';
import { ChatRoom } from '../../../core/models/chat/chat-room.model';
import { Message } from '../../../core/models/chat/message.model';
import { Observable } from 'rxjs';
import { ChatHubService } from '../../../core/services/chat-hub/chat-hub.service';
import { SaveMessageCommand } from '../../../core/models/chat/save-message-command.model';

@Component({
  selector: 'app-chatbox',
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
    ReactiveFormsModule,
    TuiInputModule
  ],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.css'
})
export class ChatboxComponent {
  chatBoxHeaderData = ChatBoxHeaderData;
  @Input() selectedRoom: ChatRoom | null = null;

  messages$: Observable<Message[]>;
  newMessage: string = '';
  isTyping: boolean = false;
  typingTimer: any;

  constructor(private chatHubService: ChatHubService) {
    // Subscribe to messages from the hub service
    this.messages$ = this.chatHubService.messages$;
  }

  ngOnChanges(changes: SimpleChanges) {
    // Reset message input when room changes
    if (changes['selectedRoom']) {
      this.newMessage = '';
    }
  }

  sendMessage() {
    if (!this.selectedRoom || !this.newMessage.trim()) return;

    const saveMessageCommand: SaveMessageCommand = {
      contentText: this.newMessage,
      roomId: this.chatHubService.roomId,
      senderId: this.chatHubService.userId,
      // attachedPostIds:,
      // media:,
    };

    // Send message through SignalR hub
    this.chatHubService.sendMessage(this.selectedRoom.id, saveMessageCommand);

    // Clear the message input
    this.newMessage = '';
  }

  onTyping() {
    if (!this.selectedRoom) return;

    // Clear previous typing timer
    if (this.typingTimer) {
      clearTimeout(this.typingTimer);
    }

    // Send typing indicator
    this.chatHubService.typingIndicator(this.selectedRoom.id, this.chatHubService.userId, true);

    // Set a timer to stop typing indicator after 3 seconds of inactivity
    this.typingTimer = setTimeout(() => {
      this.chatHubService.typingIndicator(this.selectedRoom!.id, this.chatHubService.userId, false);
    }, 3000);
  }
}
