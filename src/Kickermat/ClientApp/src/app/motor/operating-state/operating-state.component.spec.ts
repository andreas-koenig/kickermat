import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperatingStateComponent } from './operating-state.component';

describe('OperatingStateComponent', () => {
  let component: OperatingStateComponent;
  let fixture: ComponentFixture<OperatingStateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OperatingStateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OperatingStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
