import { Component, OnInit, Input, OnDestroy } from '@angular/core';

import { Subscription } from 'rxjs';

import { EnumParameter } from '@api/api.model';
import { ApiService } from '@api/api.service';

@Component({
  selector: 'oth-enum-param',
  templateUrl: './enum-param.component.html',
  styleUrls: ['./enum-param.component.scss']
})
export class EnumParamComponent implements OnInit, OnDestroy {
  @Input('param') public param!: EnumParameter;
  @Input('settings') public settings!: string;

  public subs: Subscription[] = [];
  public isUpdating = false;

  public val!: EnumParameter['value'];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.val = this.param.value;
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private updateModel(value: number): void {
    this.val = value;
    this.isUpdating = false;
  }

  public updateEnum(value: EnumParameter['value']): void {
    this.isUpdating = true;

    const sub = this.api.updateParam<EnumParameter>(
      this.settings,
      this.param.name,
      value,
      this.updateModel.bind(this),
      this.updateModel.bind(this)
    );

    this.subs.push(sub);
  }
}
