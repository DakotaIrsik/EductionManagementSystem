import { Component, OnInit, ViewChild } from '@angular/core';
import { SignalRService } from '../../services/signal-r.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NgScrollbar } from 'ngx-scrollbar';
import { ApplicationResolver } from '../../base/application.resolver';
import { BaseService } from '../../services/base-service';
@Component({
  selector: 'studio-portal-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit {
  @ViewChild(NgScrollbar, {static: true}) chatScrollBar: NgScrollbar;
  public chartOptions: any = {
    scaleShowVerticalLines: true,
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    }
  };
  public chartLabels: string[] = ['Real time data for the chart'];
  public chartType: string = 'bar';
  public chartLegend: boolean = true;
  public colors: any[] = [{ backgroundColor: '#5491DA' }, { backgroundColor: '#E74C3C' }, 
  { backgroundColor: '#82E0AA' }, { backgroundColor: '#E5E7E9' }]

  baseUrl: string;

  constructor(public signalRService: SignalRService, private http: HttpClient) {
   }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
    this.signalRService.addBroadcastChartDataListener();
    this.startHttpRequest();
  }

  private startHttpRequest = () => {
    this.http.get(this.signalRService.fullApiUrl + 'chat')
      .subscribe(res => {
        console.log(res);
      })
  }

  public chartClicked = (event) => {
    console.log(event);
    this.signalRService.broadcastChartData();
  }
}
