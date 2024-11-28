import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, SimpleChanges, ViewChild } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiAvatar, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { ChatBoxHeaderData } from '../../../core/data/chatbox-header-data';
import { TuiInputModule } from '@taiga-ui/legacy';
import { ChatRoom } from '../../../core/models/chat/chat-room.model';
import { Message } from '../../../core/models/chat/message.model';
import { map, Observable, Subject } from 'rxjs';
import { ChatHubService } from '../../../core/services/chat-hub/chat-hub.service';
import { SaveMessageCommand } from '../../../core/models/chat/save-message-command.model';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { OrderByPipe } from '../../../core/pipes/order-by/orderby.pipe';

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
    TuiInputModule,
    OrderByPipe
  ],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.css'
})
export class ChatboxComponent {
  @ViewChild('chatboxContent') private chatboxContent: ElementRef;

  chatBoxHeaderData = ChatBoxHeaderData;
  @Input() selectedRoom: ChatRoom | null = null;

  messages$: Observable<Message[]> = new Observable<Message[]>();
  reversedMessages$: Observable<Message[]> ;

  newMessage: string = '';
  isTyping: boolean = false;
  typingTimer: any;

  userReceiver: UserResponseModel;
  userId: string;
  isSent: boolean = false;
  constructor(private chatHubService: ChatHubService) {
    this.messages$ = this.chatHubService.messages$;
    console.log(this.messages$);
    
    this.reversedMessages$ = this.messages$.pipe(map((messages) => messages.filter((message) => message.createdAt).reverse()));
  }



  ngOnInit() {
    this.scrollToBottom();

  }

 

  ngOnChanges(changes: SimpleChanges) {
    this.getUserReceiver();
    // Reset message input when room changes
    if (changes['selectedRoom']) {
      this.newMessage = '';
    }

  }

  sendMessage() {
    if (!this.selectedRoom || !this.newMessage.trim()) return;

    const saveMessageCommand: SaveMessageCommand = {
      contentText: this.newMessage,
      roomId: this.selectedRoom?.id,
      senderId: this.chatHubService.userId,
      // attachedPostIds:,
      // media:,
    };

    // Send message through SignalR hub
    this.chatHubService.sendMessage(this.selectedRoom.id, saveMessageCommand);
    this.newMessage = '';

  }

  private scrollToBottom(): void {
    try {
      if (this.chatboxContent) {
        this.chatboxContent.nativeElement.scrollTop = this.chatboxContent.nativeElement.scrollHeight;
      }
    } catch (err) {
      console.error('Scroll to bottom error:', err);
    }
  }
  
  getUserReceiver() {
    if (this.selectedRoom?.participants && this.chatHubService.userId) {
      this.selectedRoom?.participants.forEach((participant) => {
        if (participant.id !== this.chatHubService.userId) {
          this.userReceiver = participant;
          this.userId = participant.id;
        }
      })
    }
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
