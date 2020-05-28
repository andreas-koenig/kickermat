import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperatingModeComponent } from './operating-mode.component';

describe('OperatingModeComponent', () => {
  let component: OperatingModeComponent;
  let fixture: ComponentFixture<OperatingModeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OperatingModeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OperatingModeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
