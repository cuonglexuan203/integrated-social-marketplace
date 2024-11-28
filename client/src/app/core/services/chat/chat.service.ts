import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/endpoint';
import { Observable } from 'rxjs';
import { ChatRoom } from '../../models/chat/chat-room.model';
import { Message } from '../../models/chat/message.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiBase = environment.apiChat;

  constructor(private http: HttpClient) { }

  getUserRooms(userId: string): Observable<ChatRoom[]> {
    return this.http.get<ChatRoom[]>(`${this.apiBase}/GetUserRooms/${userId}`);
  }

  searchMessages(roomId: string, keyword: string): Observable<Message[]> {
    return this.http.post<Message[]>(`${this.apiBase}/SearchMessages`, { roomId, keyword });
  }
}
