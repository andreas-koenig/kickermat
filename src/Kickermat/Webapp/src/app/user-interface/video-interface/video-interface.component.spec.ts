import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoInterfaceComponent } from './video-interface.component';

describe('VideoPlayerComponent', () => {
  let component: VideoInterfaceComponent;
  let fixture: ComponentFixture<VideoInterfaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VideoInterfaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideoInterfaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
