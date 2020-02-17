import { Component, Input, ElementRef, ViewChild, OnDestroy, OnInit } from '@angular/core';
import { VideoSource, Channel } from '@api/api.model';
import { CAMERA } from '@api/api';
import { ApiService } from '@api/api.service';
import { Observable } from 'rxjs';

enum CameraStatus {
  Loading, Success, Failure
}

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements OnDestroy, OnInit {
  @Input('videoSource') public videoSource = VideoSource.Camera;
  @Input('videoHeight') public videoHeight = "90vh";

  @ViewChild('img', { static: true }) public img!: ElementRef<HTMLImageElement>;

  public status = CameraStatus.Loading;
  public cameraStatusEnum = CameraStatus;
  public cameraSrc = CAMERA;

  public selectedChannel: Channel | undefined;
  public channels: Observable<Channel[]> | undefined;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getChannel(this.videoSource).subscribe(
      (channel) => this.selectedChannel = channel,
      (error) => console.log(error)
    );
    this.channels = this.api.getChannels(this.videoSource);
  }

  ngOnDestroy() {
    this.img.nativeElement.src = "";
  }

  public retry() {
    this.status = CameraStatus.Loading;
    this.img.nativeElement.src = this.cameraSrc;
  }

  public error() {
    this.status = CameraStatus.Failure;
  }

  public loaded() {
    if (this.status !== CameraStatus.Success) {
      this.status = CameraStatus.Success;
    }
  }

  public getName(videoSource: VideoSource): string {
    switch (videoSource) {
      case VideoSource.Calibration:
        return "Calibration";
      case VideoSource.Camera:
        return "Camera";
      case VideoSource.ImageProcessing:
        return "Image Processing";
    }
  }

  public switchChannel(channel: Channel) {
    this.api.switchChannel(this.videoSource, channel).subscribe(
      (channel) => this.selectedChannel = channel,
      (error) => console.log(error)
    );
  }

  public isSelected(channel: Channel): boolean {
    if (this.selectedChannel) {
      return channel.id == this.selectedChannel.id;
    }

    return false;
  }
}
