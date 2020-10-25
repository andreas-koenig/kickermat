import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { BooleanParameter } from '@api/model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'oth-boolean-param',
  templateUrl: './boolean-param.component.html',
})
export class BooleanParameterComponent implements OnInit, OnDestroy {
  @Input('param') public param!: BooleanParameter;
  @Input('settingsId') public settingsId!: string;

  public subs: Subscription[] = [];
  public isUpdating = false;

  public val!: boolean;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.val = this.param.value;
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private updateModel(value: boolean): void {
    this.val = value;
    this.isUpdating = false;
  }

  public updateBoolean(value: boolean): void {
    this.isUpdating = true;

    const sub = this.api.updateParam<BooleanParameter>(
      this.settingsId,
      this.param.name,
      value,
      !value,
      this.updateModel.bind(this),
      this.updateModel.bind(this));

    this.subs.push(sub);
  }
}
