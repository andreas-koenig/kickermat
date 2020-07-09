import { Component, Input, ElementRef, ViewChild, OnDestroy, OnInit, AfterViewChecked, Renderer2 } from '@angular/core';
import { VideoChannel, KickermatPlayer } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { VIDEO_URL } from '@api/api';
import { Subscription } from 'rxjs';

enum VideoStatus {
  Loading, Success, Failure
}

@Component({
  selector: 'app-video-interface',
  templateUrl: './video-interface.component.html',
  styleUrls: ['./video-interface.component.scss']
})
export class VideoInterfaceComponent implements OnDestroy, OnInit, AfterViewChecked {
  @Input('player') public player!: KickermatPlayer;

  @ViewChild("imgcontainer", { static: false }) public container!: ElementRef<HTMLDivElement>;
  @ViewChild("img", { static: false }) public img!: ElementRef<HTMLImageElement>;
  public videoUrl = `${VIDEO_URL}?player=`;

  public status = VideoStatus.Loading;
  public videoStatusEnum = VideoStatus;

  public currentChannel: VideoChannel | undefined;
  public channels: VideoChannel[] | undefined;

  private subs: Subscription[] = [];

  constructor(
    private api: ApiService,
    private renderer: Renderer2,
  ) { }

  ngOnInit() {
    const sub = this.api.getVideoChannels(this.player).subscribe(
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
    this.img && (this.img.nativeElement.src = VIDEO_URL);
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
    const sub = this.api.switchVideoChannel(this.player, channel)
      .subscribe(() => this.currentChannel = channel);

    this.subs.push(sub);
  }

  public isSelected(channel: VideoChannel): boolean {
    if (this.currentChannel) {
      return channel.name == this.currentChannel.name;
    }

    return false;
  }
}
