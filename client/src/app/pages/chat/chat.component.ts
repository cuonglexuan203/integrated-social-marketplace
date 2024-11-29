import { Component, OnInit, OnDestroy, Output } from '@angular/core';
import { SidebarChatComponent } from './sidebar-chat/sidebar-chat.component';
import { ChatboxComponent } from './chatbox/chatbox.component';
import { ChatService } from '../../core/services/chat/chat.service';
import { ChatRoom } from '../../core/models/chat/chat-room.model';
import { NbAuthService } from '@nebular/auth';
import { Subscription } from 'rxjs';
import { ChatHubService } from '../../core/services/chat-hub/chat-hub.service';
import { ActivatedRoute } from '@angular/router';
import { UserResponseModel } from '../../core/models/user/user.model';
import { UserService } from '../../core/services/user/user.service';
import { TuiSkeleton } from '@taiga-ui/kit';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    SidebarChatComponent,
    ChatboxComponent,
    TuiSkeleton
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit, OnDestroy {
  @Output() rooms: ChatRoom[] = [];
  @Output() userReceiver: UserResponseModel;

  receiverId: string;
  userId: string;
  selectedRoom: ChatRoom | null = null;

  isLoading: boolean = false;

  private tokenSubscription: Subscription;
  private roomSubscription: Subscription;

  constructor(
    private chatService: ChatService,
    private chatHubService: ChatHubService,
    private authService: NbAuthService,
    private activatedRoute: ActivatedRoute,
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.setupData();
  }

  async setupData
    () {
    await this.initializeUser();
    this.getReceiverId();
  }

  ngOnDestroy(): void {
    this.chatHubService.disconnectFromHub();

    if (this.tokenSubscription) {
      this.tokenSubscription.unsubscribe();
    }
    if (this.roomSubscription) {
      this.roomSubscription.unsubscribe();
    }
  }

  getReceiverId() {
    this.activatedRoute.params.subscribe(params => {
      this.receiverId = params['receiverId'];
    });
  }

  async initializeUser() {
    this.tokenSubscription = this.authService.onTokenChange().subscribe(async (token) => {
      if (token?.isValid()) {
        this.userId = token?.getPayload()?.userId;

        await this.chatHubService.connectToHub(this.userId);
        await this.joinRoom();
        this.loadUserRooms();
      }
    });
  }

  loadUserRooms() {
    this.isLoading = true;
    this.roomSubscription = this.chatService.getUserRooms(this.userId).subscribe({
      next: (res) => {
        if (res) {
          this.rooms = res
          if (this.rooms.length > 0 && !this.receiverId) {
            this.selectRoom(this.rooms[0]);
          }
          if (this.receiverId) {
            this.handleSelectRoom();
          }
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }



  async joinRoom() {
    if (this.userId && this.receiverId) {
      await this.chatHubService.joinRoom(this.userId, this.receiverId).finally(() => {
        this.handleSelectRoom();

      });
    }
  }


  handleSelectRoom() {
    if (this.rooms) {
      this.rooms.forEach(room => {
        if (room?.id === this.chatHubService.roomId) {
          this.selectedRoom = room;
        }
      });
    }
  }

  selectRoom(room: ChatRoom) {
    // If we're selecting a different room, leave the current room
    if (this.selectedRoom && this.selectedRoom.id !== room.id) {
      // Leave the previous room
      this.chatHubService.disconnectFromHub();
    }

    // Set the selected room
    this.selectedRoom = room;

    // Join the new room
    const otherUser = room.participants?.filter(p => p.id != this.userId);
    this.chatHubService.joinRoom(this.userId, otherUser?.[0]?.id);
  }

  
}