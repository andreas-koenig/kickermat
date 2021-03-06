import { Component, OnInit, OnDestroy, TemplateRef, ViewChild, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  Settings, KickerParameter, NumberParameter, ColorRangeParameter,
  BooleanParameter, EnumParameter } from '@api/model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

import util from './params-util';

type Status = 'loading' | 'done' | 'error';

@Component({
  selector: 'oth-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy, OnChanges {
  @Input('id') public id?: string;

  public settings!: Settings[];
  public subscriptions: Subscription[] = [];
  public loaded = false;
  public status: Status = 'loading';

  public isExpanded: { [key: number]: boolean } = {};

  // Parameter templates
  @ViewChild('numberTmpl', { static: true })
  public numberTmpl!: TemplateRef<NumberParameter>;

  @ViewChild('colorRangeTmpl', { static: true })
  public colorRangeTmpl!: TemplateRef<ColorRangeParameter>;

  @ViewChild('booleanTmpl', { static: true })
  public booleanTmpl!: TemplateRef<BooleanParameter>;

  @ViewChild('enumTmpl', { static: true })
  public enumTmpl!: TemplateRef<EnumParameter>;

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.getSettings();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.id) {
      this.getSettings();
    }
  }

  getSettings(): void {
    if (!this.id) {
      return;
    }

    const sub = this.api
      .getSettings(this.id)
      .subscribe(
        settings => {
          this.settings = settings;
          this.settings.forEach((s, i) => this.isExpanded[i] = true);
          this.loaded = true;
          this.status = 'done';
        },
        error => {
          this.status = 'error';
          console.log(`Could not fetch settings for id ${this.id}: ${error}`);
        });

    this.subscriptions.push(sub);
  }

  getParamTemplate(param: KickerParameter<any>):
      TemplateRef<KickerParameter<any>> | undefined {
    if (util.isNumberParam(param)) {
      return this.numberTmpl;
    } else if (util.isColorRangeParam(param)) {
      return this.colorRangeTmpl;
    } else if (util.isBooleanParam(param)) {
      return this.booleanTmpl;
    } else if (util.isEnumParam(param)) {
      return this.enumTmpl;
    }
  }

  toggleExpansion(i: number): void {
    this.isExpanded[i] = !this.isExpanded[i];
  }
}
