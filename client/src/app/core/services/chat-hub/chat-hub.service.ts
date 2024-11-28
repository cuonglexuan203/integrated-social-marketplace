import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { Message } from '../../models/chat/message.model';
import { environment } from '../../../../environments/endpoint';
import { Pagination } from '../../models/page/pagination.model';
import { NbAuthService } from '@nebular/auth';

@Injectable({
  providedIn: 'root',
})
export class ChatHubService {
  private chatApiBase = environment.apiChat;
  private hubConnection!: signalR.HubConnection;
  private messagesSource = new BehaviorSubject<Message[]>([]);
  messages$ = this.messagesSource.asObservable();

  private typingSource = new BehaviorSubject<{ userId: string; isTyping: boolean } | null>(null);
  typing$ = this.typingSource.asObservable();

  roomId: string;
  private token: string;
  userId: string;

  constructor(private authService: NbAuthService) {
    this.initChatHub();
  }

  connectToHub(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.chatApiBase}/chatHub`, {
        accessTokenFactory: () => this.token || '',
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error('SignalR Error:', err));

    // Load message history
    this.hubConnection.on("ReceiveMessageHistory", (roomId: string, messageHistoryPage: Pagination<Message>) => {
      this.roomId = roomId;
      this.messagesSource.next(messageHistoryPage.data);
    });

    // Listen to messages
    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      const currentMessages = this.messagesSource.value;
      this.messagesSource.next([...currentMessages, message]);
    });

    // Listen for typing indicator ...
    this.hubConnection.on('TypingIndicator', (userId: string, isTyping: boolean) => {
      this.typingSource.next({ userId, isTyping });
    });
  }

  disconnectFromHub() {
    this.hubConnection.invoke("LeaveRoom");
    this.hubConnection.stop();
  }

  joinRoom(userId: string, otherUserId: string) {
    this.hubConnection
      .invoke('ConnectToRoom', userId, otherUserId)
      .catch(err => console.error('Error connecting to room:', err));
  }

  sendMessage(roomId: string, saveMessageCommand: SaveMessageCommand) {
    this.hubConnection.invoke('SendMessage', roomId, saveMessageCommand).catch(err => {
      console.error('Error sending message:', err);
    });
  }

  typingIndicator(roomId: string, userId: string, isTyping: boolean) {
    this.hubConnection.invoke('TypingIndicator', roomId, userId, isTyping).catch(err => {
      console.error('Error sending typing indicator:', err);
    });
  }

  initChatHub() {
    this.authService.onTokenChange().subscribe((token) => {
      if (token?.isValid()) {
        this.token = token.getValue();
        this.userId = token?.getPayload()?.userId;
      }
    });
  }
}
