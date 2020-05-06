import { Component } from '@angular/core';
import { KickerParameterComponent } from '../kicker-parameter';
import { NzMessageService } from 'ng-zorro-antd';
import { ApiService } from '@api/api.service';
import { NumberParameter } from '@api/api.model';

@Component({
  selector: 'app-number-parameter',
  templateUrl: './number-parameter.component.html',
  styleUrls: ['./number-parameter.component.scss']
})
export class NumberParameterComponent extends KickerParameterComponent<NumberParameter> {
  public num: number | undefined;

  constructor(api: ApiService, message: NzMessageService) {
    super(api, message);
  }

  protected updateModel(value: number) {
    this.num = value;
  }
}
