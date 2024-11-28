import { Component, OnInit, OnDestroy } from '@angular/core';
import { SidebarChatComponent } from './sidebar-chat/sidebar-chat.component';
import { ChatboxComponent } from './chatbox/chatbox.component';
import { ChatService } from '../../core/services/chat/chat.service';
import { ChatRoom } from '../../core/models/chat/chat-room.model';
import { NbAuthService } from '@nebular/auth';
import { Subscription } from 'rxjs';
import { ChatHubService } from '../../core/services/chat-hub/chat-hub.service';

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
export class ChatComponent implements OnInit, OnDestroy {
  rooms: ChatRoom[] = [];
  userId: string;
  selectedRoom: ChatRoom | null = null;

  private tokenSubscription: Subscription;
  private roomSubscription: Subscription;

  constructor(
    private chatService: ChatService,
    private chatHubService: ChatHubService,
    private authService: NbAuthService
  ) {}

  ngOnInit(): void {
    this.initializeUser();
  }

  ngOnDestroy(): void {
    // Disconnect from hub
    this.chatHubService.disconnectFromHub();

    // Unsubscribe from all subscriptions
    if (this.tokenSubscription) {
      this.tokenSubscription.unsubscribe();
    }
    if (this.roomSubscription) {
      this.roomSubscription.unsubscribe();
    }
  }

  private initializeUser() {
    this.tokenSubscription = this.authService.onTokenChange().subscribe(async (token) => {
      if (token?.isValid()) {
        this.userId = token?.getPayload()?.userId;
        
        // Connect to SignalR hub
        await this.chatHubService.connectToHub(this.userId);
        // Load user rooms
        this.loadUserRooms();
      }
    });
  }

  private loadUserRooms() {
    this.roomSubscription = this.chatService.getUserRooms(this.userId).subscribe(rooms => {
      this.rooms = rooms;
      
      // Automatically select the first room if available
      if (rooms.length > 0) {
        this.selectRoom(rooms[0]);
      }
    });
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