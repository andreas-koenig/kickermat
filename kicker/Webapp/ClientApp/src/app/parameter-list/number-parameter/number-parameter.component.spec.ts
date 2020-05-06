import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NumberParameterComponent } from './number-parameter.component';

describe('NumberParameterComponent', () => {
  let component: NumberParameterComponent;
  let fixture: ComponentFixture<NumberParameterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NumberParameterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NumberParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
