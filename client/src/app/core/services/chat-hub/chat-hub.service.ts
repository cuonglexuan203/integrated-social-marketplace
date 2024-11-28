import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { Message } from '../../models/chat/message.model';
import { environment } from '../../../../environments/endpoint';
import { Pagination } from '../../models/page/pagination.model';
import { NbAuthService } from '@nebular/auth';
import { SaveMessageCommand } from '../../models/chat/save-message-command.model';

@Injectable({
  providedIn: 'root',
})
export class ChatHubService {
  private chatHubApiBase = environment.apiChatHub;
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

  async connectToHub(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.chatHubApiBase, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.token || '',
      })
      .withAutomaticReconnect([0, 1000, 5000, 10000])
      .build();
    try {
      await this.hubConnection.start();
      console.log("Connected to chat hub")
    }
    catch (err) {
      console.error('SignalR Error:', err)
      return;
    }

    this.setupHubListeners();
  }


  private setupHubListeners() {
    this.hubConnection.on("ReceiveMessageHistory", this.handleMessageHistory.bind(this));
    this.hubConnection.on('ReceiveMessage', this.handleNewMessage.bind(this));
    this.hubConnection.on('TypingIndicator', this.handleTypingIndicator.bind(this));
  }

  private handleMessageHistory(roomId: string, messageHistoryPage: Pagination<Message>) {
    this.roomId = roomId;
    this.messagesSource.next(messageHistoryPage.data);
  }

  private handleNewMessage(message: Message) {
    const currentMessages = this.messagesSource.value;
    this.messagesSource.next([...currentMessages, message]);
  }

  private handleTypingIndicator(userId: string, isTyping: boolean) {
    this.typingSource.next({ userId, isTyping });
  }

  //

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