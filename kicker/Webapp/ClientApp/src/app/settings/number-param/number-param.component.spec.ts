import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NumberParamComponent } from './number-param.component';

describe('NumberParamComponent', () => {
  let component: NumberParamComponent;
  let fixture: ComponentFixture<NumberParamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NumberParamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NumberParamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
