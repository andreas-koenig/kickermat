import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KickerParameterListComponent } from './kicker-parameter-list.component';

describe('KickerParameterListComponent', () => {
  let component: KickerParameterListComponent;
  let fixture: ComponentFixture<KickerParameterListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KickerParameterListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KickerParameterListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
