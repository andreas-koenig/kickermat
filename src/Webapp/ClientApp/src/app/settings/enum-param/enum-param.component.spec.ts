import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnumParamComponent } from './enum-param.component';

describe('EnumParamComponent', () => {
  let component: EnumParamComponent;
  let fixture: ComponentFixture<EnumParamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnumParamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EnumParamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
