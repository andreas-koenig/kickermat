import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorRangeParameterComponent } from './color-range-param.component';

describe('ColorRangeParameterComponent', () => {
  let component: ColorRangeParameterComponent;
  let fixture: ComponentFixture<ColorRangeParameterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorRangeParameterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorRangeParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
