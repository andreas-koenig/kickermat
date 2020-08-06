import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { NumberParameter } from '@api/api.model';
import { NzMarks } from 'ng-zorro-antd';
import { Subscription } from 'rxjs';
import { ApiService } from '../../../api/api.service';

@Component({
  selector: 'app-number-param',
  templateUrl: './number-param.component.html',
  styleUrls: ['./number-param.component.scss']
})
export class NumberParamComponent implements OnInit, OnDestroy {
  @Input("param") public param!: NumberParameter;
  @Input("settings") public settings!: string;
  public defaultMark: NzMarks = {};

  public subs: Subscription[] = [];
  public isUpdating = false;

  public num: number | undefined;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.defaultMark[this.param.defaultValue] = `Default: ${this.param.defaultValue}`
    this.num = this.param.value;
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private updateModel(value: number) {
    this.num = value;
    this.isUpdating = false;
  }

  public updateNumber(value: number[] | number): void {
    if (Array.isArray(value)) {
      return;
    }

    this.isUpdating = true;

    const sub = this.api.updateParam<NumberParameter>(
      this.settings,
      this.param.name,
      value,
      this.updateModel.bind(this),
      this.updateModel.bind(this));

    this.subs.push(sub);
  }
}
