import { Component, Input, ElementRef, ViewChild, OnDestroy, OnInit, AfterViewChecked, Renderer2, OnChanges } from '@angular/core';
import { VideoChannel, KickermatPlayer, Camera } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { UI_VIDEO_URL, CAMERA_VIDEO_URL } from '@api/api';
import { Subscription } from 'rxjs';

enum VideoStatus {
  Loading, Success, Failure
}

@Component({
  selector: 'app-video-interface',
  templateUrl: './video-interface.component.html',
  styleUrls: ['./video-interface.component.scss']
})
export class VideoInterfaceComponent implements OnDestroy, OnInit, AfterViewChecked, OnChanges {
  @Input('videoSource') public videoSource!: KickermatPlayer | Camera;

  @ViewChild("imgcontainer", { static: false }) public container!: ElementRef<HTMLDivElement>;
  @ViewChild("img", { static: false }) public img!: ElementRef<HTMLImageElement>;
  public videoUrl: string = "";

  public status = VideoStatus.Loading;
  public videoStatusEnum = VideoStatus;

  public currentChannel: VideoChannel | undefined;
  public channels: VideoChannel[] = [];

  private subs: Subscription[] = [];

  constructor(
    private api: ApiService,
    private renderer: Renderer2,
  ) { }

  ngOnInit() {
    if (!this.isCamera()) {
      this.getChannels();
    }
  }

  ngOnChanges() {
    this.videoUrl = this.isCamera()
      ? this.videoUrl = `${CAMERA_VIDEO_URL}?camera=${this.videoSource.name}`
      : this.videoUrl = `${UI_VIDEO_URL}?player=${this.videoSource.name}`;
  }

  ngAfterViewChecked() {
    if (this.container) {
      const width = this.container.nativeElement.offsetWidth;
      const height = (width / 16) * 9;
      const heightValue = height === 0 ? "100%" : `${height}px`;
      this.renderer.setStyle(this.container.nativeElement, "height", heightValue);
    }
  }

  ngOnDestroy() {
    // Set source to empty string to close connection
    // to video endpoint
    this.img && (this.img.nativeElement.src = "");

    this.subs.forEach(sub => sub.unsubscribe());
  }

  public retry() {
    this.status = VideoStatus.Loading;
    this.img && (this.img.nativeElement.src = this.videoUrl);
  }

  public error() {
    this.status = VideoStatus.Failure;
  }

  public loaded() {
    if (this.status !== VideoStatus.Success) {
      this.status = VideoStatus.Success;
    }
  }

  public switchChannel(channel: VideoChannel) {
    const sub = this.api
      .switchVideoChannel(this.videoSource as KickermatPlayer, channel)
      .subscribe(() => this.currentChannel = channel);

    this.subs.push(sub);
  }

  public isSelected(channel: VideoChannel): boolean {
    if (this.currentChannel) {
      return channel.name == this.currentChannel.name;
    }

    return false;
  }

  public isCamera(): boolean {
    return (this.videoSource as any).peripheralState ? true : false;
  }

  private getChannels() {
    const sub = this.api
      .getVideoChannels(this.videoSource as KickermatPlayer)
      .subscribe(
        resp => {
          this.channels = resp.channels;
          this.currentChannel = resp.currentChannel;
        },
        error => {
          this.status = VideoStatus.Failure;
          console.log(error);
        }
      );

    this.subs.push(sub);
  }
}
