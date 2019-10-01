import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KickerParameterComponent } from './kicker-parameter.component';

describe('KickerParameterComponent', () => {
  let component: KickerParameterComponent;
  let fixture: ComponentFixture<KickerParameterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KickerParameterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KickerParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
