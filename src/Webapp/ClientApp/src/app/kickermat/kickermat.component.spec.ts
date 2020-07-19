import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KickermatComponent } from './kickermat.component';

describe('KickermatComponent', () => {
  let component: KickermatComponent;
  let fixture: ComponentFixture<KickermatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KickermatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KickermatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
