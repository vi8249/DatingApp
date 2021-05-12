import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUsl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMessages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);
    console.log(params);
    return getPaginatedResult<Message[]>(this.baseUsl + "messages", params, this.http);
  }

  getMessagesThread(username: string) {
    return this.http.get<Message[]>(this.baseUsl + "messages/thread/" + username);
  }

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(this.baseUsl + "messages", { recipientUsername: username, content });
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUsl + "messages/" + id);
  }

}
