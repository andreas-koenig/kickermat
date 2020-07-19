import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NmtStateComponent } from './nmt-state.component';

describe('NmtStateComponent', () => {
  let component: NmtStateComponent;
  let fixture: ComponentFixture<NmtStateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NmtStateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NmtStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
