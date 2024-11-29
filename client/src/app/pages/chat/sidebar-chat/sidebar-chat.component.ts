import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SidebarChatHeaderData } from '../../../core/data/sidebar-chat-header-data';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { TuiAvatar, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChatService } from '../../../core/services/chat/chat.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { NbAuthService } from '@nebular/auth';
import { ChatRoom } from '../../../core/models/chat/chat-room.model';
import { ChatHubService } from '../../../core/services/chat-hub/chat-hub.service';

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
  @Input() rooms: ChatRoom[];
  @Input() selectedRoom: ChatRoom | null;

  sidebarChatHeaderData = SidebarChatHeaderData;
  userMockData: any[] = []

  unRead = false;

  userId: string;
  userReceiver: UserResponseModel;

  @Output() selectedRoomFromSidebar: EventEmitter<ChatRoom> = new EventEmitter<ChatRoom>();
  constructor(
    private _chatService: ChatService,
    private authService: NbAuthService,
    private chatHubService: ChatHubService
  ) { }

  ngOnInit() {
    this.setupData();
  }


  getUserReceiver() {
    if (this.selectedRoom?.participants && this.userId) {
      this.selectedRoom?.participants.forEach((participant) => {
        if (participant.id !== this.userId) {
          this.userReceiver = participant;
        }
      })
    }
  }

  async getUserId() {
    this.authService.onTokenChange().subscribe((token) => {
      if (token?.isValid()) {
        this.userId = token?.getPayload()?.userId
      }
    })
  }

  async setupData() {
    await this.getUserId();
    this.getUserReceiver();
    
  }

  ngOnChanges() {
    this.getUserReceiver();
  }

  selectRoom(room: ChatRoom) {
    this.selectedRoomFromSidebar.emit(room);
  }



}
