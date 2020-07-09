import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KickerComponent } from './kicker.component';

describe('KickermatComponent', () => {
  let component: KickerComponent;
  let fixture: ComponentFixture<KickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
