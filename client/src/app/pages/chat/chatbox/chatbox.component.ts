import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, SimpleChanges, ViewChild } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiDropdown, TuiDropdownOpen, TuiIcon, TuiTextfield } from '@taiga-ui/core';
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
import { AlertService } from '../../../core/services/alert/alert.service';

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
    OrderByPipe,
    TuiDropdown,
    TuiDropdownOpen,
  ],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.css'
})
export class ChatboxComponent {
  @ViewChild('chatboxContent') private chatboxContent: ElementRef;

  chatBoxHeaderData = ChatBoxHeaderData;
  @Input() selectedRoom: ChatRoom | null = null;

  messages$: Observable<Message[]> = new Observable<Message[]>();
  reversedMessages$: Observable<Message[]>;

  newMessage: string = '';
  isTyping: boolean = false;
  typingTimer: any;

  userReceiver: UserResponseModel;
  userId: string;
  isSent: boolean = false;

  postUrl: string;

  typingUser: UserResponseModel | null = null;
  selectedFiles: File[] = [];
  filePreviewUrls: string[] = [];

  constructor(
    private chatHubService: ChatHubService,
    private alertService: AlertService
  ) {
    this.messages$ = this.chatHubService.messages$.pipe(
      map(messages => messages.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()))
    );

    this.chatHubService.typing$.subscribe(typingInfo => {
      if (typingInfo && typingInfo.userId !== this.chatHubService.userId) {
        // Find the typing user from room participants
        this.typingUser = this.selectedRoom?.participants.find(
          participant => participant.id === typingInfo.userId
        ) || null;
      } else {
        this.typingUser = null;
      }
    });
  }

  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
    this.filePreviewUrls.splice(index, 1);
  }

  ngOnInit() {
    this.scrollToBottom();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'video/mp4', 'video/webm'];
      
      for (let i = 0; i < input.files.length; i++) {
        const file = input.files[i];
        
        if (allowedTypes.includes(file.type)) {
          this.selectedFiles.push(file);
          
          // Create preview URL
          const reader = new FileReader();
          reader.onload = (e) => {
            this.filePreviewUrls.push(e.target?.result as string);
          };
          reader.readAsDataURL(file);
        }
      }
    }
    else {
      this.alertService.showError('Please select a valid image or video file', 'Invalid file type');
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedRoom']) {
      const currentRoom = changes['selectedRoom'].currentValue;
      const previousRoom = changes['selectedRoom'].previousValue;
  
      if (currentRoom && currentRoom !== previousRoom) {
        this.newMessage = '';
        this.selectedFiles = [];
        this.filePreviewUrls = [];
        this.scrollToBottom(); // Ensures proper scrolling on room change
        this.getUserReceiver(); // Update receiver info for the new room
      }
    }
  }

  private scrollToBottom(): void {
    if (!this.chatboxContent) return;
  
    setTimeout(() => {
      const contentElement = this.chatboxContent.nativeElement;
      contentElement.scrollTop = contentElement.scrollHeight;
    });
  }

  sendMessage() {
    console.log(this.selectedRoom);
    
    if (!this.selectedRoom || !this.newMessage.trim()) return;

    const saveMessageCommand: SaveMessageCommand = {
      contentText: this.newMessage,
      roomId: this.selectedRoom?.id,
      senderId: this.chatHubService.userId,
      attachedPostIds: this.postUrl ? [this.postUrl] : [],
      media: this.selectedFiles
    };

    // Send message through SignalR hub
    this.chatHubService.sendMessage(this.selectedRoom.id, saveMessageCommand);
    if (this.selectedRoom?.messagePage?.data[0]) {
      this.selectedRoom.messagePage.data[0].contentText = this.newMessage;
    }
    this.newMessage = '';

  }

  getUserReceiver() {
    if (this.selectedRoom?.participants && this.chatHubService.userId) {
      this.selectedRoom?.participants.forEach((participant) => {
        if (participant.id !== this.chatHubService.userId) {
          this.userReceiver = participant;
          this.userId = participant.id;
        }
      });
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