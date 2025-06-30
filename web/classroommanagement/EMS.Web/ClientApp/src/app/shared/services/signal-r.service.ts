import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ChatModel } from '../models/chat';
import { BaseService } from './base-service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalRService extends BaseService {
  public data: ChatModel[] = [];
  public bradcastedData: ChatModel[];

  constructor(private http: HttpClient) {
    super();
  }

private hubConnection: signalR.HubConnection;
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl( this.apiUrl + 'chat')
                            .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addTransferChartDataListener() {

    this.hubConnection.on('transferchartdata', (data) => {
      if (this.data.length >= 10) {
        this.data.shift();
      }
      this.data.push(data);
      console.log(data);
    });
  }

  public broadcastChartData = () => {
    this.hubConnection.invoke('broadcastchartdata', this.data)
    .catch(err => console.error(err));
  }

  public addBroadcastChartDataListener = () => {
    this.hubConnection.on('broadcastchartdata', (data) => {
      this.bradcastedData = data;
    });
  }
}


