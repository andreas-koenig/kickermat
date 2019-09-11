import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MjpegVideoPlayerComponent } from './mjpeg-video-player.component';

describe('MjpegVideoPlayerComponent', () => {
  let component: MjpegVideoPlayerComponent;
  let fixture: ComponentFixture<MjpegVideoPlayerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MjpegVideoPlayerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MjpegVideoPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
