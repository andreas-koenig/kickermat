import { Component, Input, ElementRef, ViewChild, OnDestroy, OnInit, AfterViewChecked, Renderer2, OnChanges } from '@angular/core';
import { VideoChannel, KickermatPlayer, Camera } from '@api/model';
import { ApiService } from '@api/api.service';
import { UI_VIDEO_URL, CAMERA_VIDEO_URL } from '@api/api';
import { Subscription } from 'rxjs';

enum VideoStatus {
  Loading, Success, Failure
}

type Status = 'loading' | 'success' | 'error';

@Component({
  selector: 'oth-video-interface',
  templateUrl: './video-interface.component.html',
  styleUrls: ['./video-interface.component.scss']
})
export class VideoInterfaceComponent implements OnDestroy, OnInit, OnChanges {
  @Input('videoSource') public videoSource!: KickermatPlayer | Camera;

  @ViewChild('img', { static: false }) public img!: ElementRef<HTMLImageElement>;
  public videoUrl = '';

  public status: Status = 'loading';

  public currentChannel: VideoChannel | undefined;
  public channels: VideoChannel[] = [];

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    if (!this.isCamera()) {
      this.getChannels();
    }
  }

  ngOnChanges(): void {
    this.videoUrl = this.isCamera()
      ? this.videoUrl = `${CAMERA_VIDEO_URL}?id=${this.videoSource.id}`
      : this.videoUrl = `${UI_VIDEO_URL}?playerId=${this.videoSource.id}`;
  }

  ngOnDestroy(): void {
    // Set source to empty string to close connection
    // to video endpoint
    if (this.img) {
      this.img.nativeElement.src = '';
    }

    this.subs.forEach(sub => sub.unsubscribe());
  }

  public retry(): void {
    this.status = 'loading';
    if (this.img) {
      this.img.nativeElement.src = this.videoUrl;
    }
  }

  public onError(): void {
    this.status = 'error';
  }

  public onSuccess(): void {
    if (this.status !== 'success') {
      this.status = 'success';
    }
  }

  public switchChannel(channel: VideoChannel): void {
    const sub = this.api
      .switchVideoChannel(this.videoSource as KickermatPlayer, channel)
      .subscribe(() => this.currentChannel = channel);

    this.subs.push(sub);
  }

  public isSelected(channel: VideoChannel): boolean {
    if (this.currentChannel) {
      return channel.name === this.currentChannel.name;
    }

    return false;
  }

  public isCamera(): boolean {
    return (this.videoSource as any).peripheralState ? true : false;
  }

  private getChannels(): void {
    const sub = this.api
      .getVideoChannels(this.videoSource as KickermatPlayer)
      .subscribe(
        resp => {
          this.channels = resp.channels;
          this.currentChannel = resp.currentChannel;
        },
        error => {
          this.status = 'error';
          console.log(error);
        }
      );

    this.subs.push(sub);
  }
}
