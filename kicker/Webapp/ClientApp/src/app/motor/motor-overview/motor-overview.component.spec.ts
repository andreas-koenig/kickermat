import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MotorOverviewComponent } from './motor-overview.component';

describe('MotorOverviewComponent', () => {
  let component: MotorOverviewComponent;
  let fixture: ComponentFixture<MotorOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MotorOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MotorOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
