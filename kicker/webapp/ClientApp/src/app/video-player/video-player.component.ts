import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

import { HUB_CAMERA, HUB_CAMERA_VIDEO } from '@api/api';

/*
interface IImage {
  name: string;
}
*/

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements OnInit, OnDestroy {

  private connection: HubConnection;
  public imageBase64 = "";

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl(HUB_CAMERA)
      .configureLogging(LogLevel.Debug)
      .build();
  }

  ngOnInit() {
    this.connection.start().catch(err => {
      console.log('Error while starting connection!');
      console.log(err);
    }).then(() => {
      console.log('connection established');
      this.startCameraStream();
    });
  }

  ngOnDestroy() {
    //this.connection.stop();
  }

  private startCameraStream() {
    this.connection.stream(HUB_CAMERA_VIDEO).subscribe({
      next: (img: string) => {
        //console.log(img);
        this.imageBase64 = "data:image/png;base64," + img;
      },
      complete: () => {
        console.log('complete');
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
}
