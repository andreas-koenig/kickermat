import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { VIDEOSOURCE_ENDPOINTS, VideoSourceEndpoint } from '@api/api';
import { VideoSource } from '../../api/api.model';

enum EndpointStatus {
  Error,
  Loading,
  Streaming,
  Disconnected
}

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements OnInit, OnDestroy {

  private connection: HubConnection | undefined;
  private endpoint: VideoSourceEndpoint | undefined;

  @Input('videoSource') videoSource = VideoSource.CAMERA;
  @Input('videoHeight') videoHeight = "90vh";
  public imageBase64 = "";
  public status: EndpointStatus = EndpointStatus.Loading;
  public statusEnum = EndpointStatus;
  public cameraName: string | undefined;

  constructor() {
    this.endpoint = VIDEOSOURCE_ENDPOINTS.get(this.videoSource);

    if (!this.endpoint) {
      // TODO: throw error
      return;
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(this.endpoint.hub)
      .build();
  }

  ngOnInit() {
    if (!this.connection || !this.endpoint) {
      return;
    }

    this.connection.start().catch(err => {
      console.log('[VideoPlayer]: Connection to %s failed: %O', this.videoSource, err);
      // TODO: show image
    }).then(() => {
      console.log('[VideoPlayer] Started connection to %s', this.videoSource);
      this.startCameraStream();
    });

    this.connection.on(this.endpoint.disconnected, (cameraName: string) => {
      console.log('[VideoPlayer] Camera %s disconnected', this.cameraName);
      this.status = EndpointStatus.Disconnected;
      this.cameraName = cameraName;
    });

    this.connection.on(this.endpoint.connected, (cameraName: string) => {
      console.log('[VideoPlayer] Camera %s connected', this.cameraName);
      this.cameraName = cameraName;

      if (this.status = EndpointStatus.Error) {
        this.status = EndpointStatus.Loading;
        this.startCameraStream();
      } else {
        this.status = EndpointStatus.Streaming;
      }
    });
  }

  ngOnDestroy() {
    if (this.connection) {
      this.connection.stop();
    }
  }

  public startCameraStream() {
    if (!this.connection || !this.endpoint) {
      return;
    }

    this.status = EndpointStatus.Loading;

    this.connection.stream(this.endpoint.video, VideoSource.CAMERA).subscribe({
      next: (img: string) => {
        this.imageBase64 = "data:image/png;base64," + img;
        if (this.status != EndpointStatus.Streaming) {
          this.status = EndpointStatus.Streaming;
        }
      },
      complete: () => {
        console.log('[VideoPlayer] Stream completed');
        // no action
      },
      error: (err: any) => {
        console.log('[VideoPlayer] Video stream stopped: %O', err);
        this.status = EndpointStatus.Error;
      }
    });

    console.log('[VideoPlayer] Started stream');
  }
}
