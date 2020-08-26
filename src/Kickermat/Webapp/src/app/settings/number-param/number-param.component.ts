import { Component, Input, OnInit, OnDestroy } from '@angular/core';

import { Subscription, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';

import { ApiService } from '@api/api.service';
import { NumberParameter } from '@api/api.model';

@Component({
  selector: 'oth-number-param',
  templateUrl: './number-param.component.html',
})
export class NumberParamComponent implements OnInit, OnDestroy {
  @Input('param') public param!: NumberParameter;
  @Input('settings') public settings!: string;

  public subs: Subscription[] = [];
  public isUpdating = false;

  public num: number | undefined;
  public numSubject = new Subject<number>();

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.num = this.param.value;
    const sub = this.numSubject
      .pipe(debounceTime(500), distinctUntilChanged(), skip(1))
      .subscribe(value => this.updateParam(value));

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
    this.numSubject.unsubscribe();
  }

  public numberChange(value: number): void {
    this.numSubject.next(value);
  }

  private updateModel(value: number): void {
    this.isUpdating = false;
    this.num = value;
  }

  private updateParam(value: number): void {
    if (this.isUpdating) {
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
