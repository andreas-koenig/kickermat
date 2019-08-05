import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

import { HUB_CAMERA, HUB_CAMERA_VIDEO, HUB_CAMERA_DISCONNECTED, HUB_CAMERA_CONNECTED } from '@api/api';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements OnInit, OnDestroy {

  private connection: HubConnection;
  public imageBase64 = "";
  public isDisconnected = false;
  public cameraName: string | undefined;

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
      this.startCameraStream();
    });

    this.connection.on(HUB_CAMERA_DISCONNECTED, (cameraName: string) => {
      console.log(cameraName + " disconnected");
      this.isDisconnected = true;
      this.cameraName = cameraName;
    });

    this.connection.on(HUB_CAMERA_CONNECTED, (cameraName: string) => {
      console.log(cameraName + " connected");
      this.isDisconnected = false;
      this.cameraName = cameraName;
    });
  }

  ngOnDestroy() {
    this.connection.stop();
  }

  private startCameraStream() {
    this.connection.stream(HUB_CAMERA_VIDEO).subscribe({
      next: (img: string) => {
        this.imageBase64 = "data:image/png;base64," + img;
        if (this.isDisconnected) {
          this.isDisconnected = false;
        }
      },
      complete: () => {
        // no action
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
}
