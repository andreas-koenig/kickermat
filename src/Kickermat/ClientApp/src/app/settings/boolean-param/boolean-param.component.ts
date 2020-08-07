import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { BooleanParameter } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-boolean-param',
  templateUrl: './boolean-param.component.html',
  styleUrls: ['./boolean-param.component.scss']
})
export class BooleanParameterComponent implements OnInit, OnDestroy {
  @Input("param") public param!: BooleanParameter;
  @Input("settings") public settings!: string;

  public subs: Subscription[] = [];
  public isUpdating = false;

  public val!: boolean;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.val = this.param.value;
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private updateModel(value: boolean) {
    this.val = value;
    this.isUpdating = false;
  }

  public updateBoolean(value: boolean) {
    this.isUpdating = true;

    const sub = this.api.updateParam<BooleanParameter>(
      this.settings,
      this.param.name,
      value,
      this.updateModel.bind(this),
      this.updateModel.bind(this));

    this.subs.push(sub);
  }
}
