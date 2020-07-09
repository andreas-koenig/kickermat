import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BooleanParameterComponent } from './boolean-param.component';

describe('BooleanParameterComponent', () => {
  let component: BooleanParameterComponent;
  let fixture: ComponentFixture<BooleanParameterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BooleanParameterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BooleanParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
