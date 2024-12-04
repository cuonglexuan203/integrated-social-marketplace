import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SidebarChatHeaderData } from '../../../core/data/sidebar-chat-header-data';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { TuiAvatar, TuiBadgedContent, TuiSkeleton, TuiSwitch } from '@taiga-ui/kit';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChatService } from '../../../core/services/chat/chat.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { NbAuthService } from '@nebular/auth';
import { ChatRoom } from '../../../core/models/chat/chat-room.model';
import { ChatHubService } from '../../../core/services/chat-hub/chat-hub.service';
import { UserService } from '../../../core/services/user/user.service';
import { debounceTime, Subject } from 'rxjs';

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
    ReactiveFormsModule,
    TuiSkeleton
  ],
  templateUrl: './sidebar-chat.component.html',
  styleUrl: './sidebar-chat.component.css'
})
export class SidebarChatComponent {
  @Input() rooms: ChatRoom[];
  @Output() selectedUser: EventEmitter<UserResponseModel> = new EventEmitter<UserResponseModel>();
  
  @Input() selectedRoom: ChatRoom | null;
  storedRooms: ChatRoom[];

  sidebarChatHeaderData = SidebarChatHeaderData;
  userMockData: any[] = []

  unRead = false;

  userId: string;
  userReceiver: UserResponseModel;

  searchValue: string = '';
  userSearchValue: any[] = [];
  private searchSubject: Subject<any> = new Subject<any>();

  @Output() selectedRoomFromSidebar: EventEmitter<ChatRoom> = new EventEmitter<ChatRoom>();

  isLoading: boolean = false;
  constructor(
    private _chatService: ChatService,
    private authService: NbAuthService,
    private chatHubService: ChatHubService,
    private _userService: UserService
  ) { 
  }



  ngOnInit() {
    this.setupData();
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.searchUser();
    });
    console.log(this.userSearchValue);

  }

  ngAfterContentInit() {
    this.storedRooms = this.rooms;
  }

  searchUser() {
    if (this.searchValue) {
      this.isLoading = true;
      this._userService.searchUserFullName(this.searchValue).subscribe({
        next: (res) => {
          this.userSearchValue = res?.result?.data;
          this.isLoading = false;
        },
        error: (err) => {
          this.isLoading = false;
          console.log(err);
        },
        complete: () => {
          this.isLoading = false;
        }
      })
    }
  }

  ngOnDestroy() {
    this.searchSubject.complete();
  }


  onSearchTyping() {
    this.searchSubject.next(this.searchValue);
    if (!this.searchValue) {
      this.userSearchValue = [];
    }
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


  selectUser(user: UserResponseModel) {
    this.selectedUser.emit(user);
  }
}
