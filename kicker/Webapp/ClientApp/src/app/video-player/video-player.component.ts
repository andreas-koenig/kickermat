import { Component, Input, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { VideoSource } from '@api/api.model';
import { CAMERA } from '@api/api';

enum CameraStatus {
  Loading, Success, Failure
}

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements OnDestroy {
  @Input('videoSource') public videoSource = VideoSource.Camera;
  @Input('videoHeight') public videoHeight = "90vh";

  @ViewChild('img', { static: true }) public img!: ElementRef<HTMLImageElement>;

  public status = CameraStatus.Loading;
  public cameraStatusEnum = CameraStatus;
  public cameraSrc = CAMERA;

  constructor() { }

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
}
