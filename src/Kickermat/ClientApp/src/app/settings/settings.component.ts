import { Component, OnInit, OnDestroy, TemplateRef, ViewChild, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  Settings, KickerParameter, NumberParameter, ColorRangeParameter,
  BooleanParameter, EnumParameter } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

import util from './params-util';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy, OnChanges {
  @Input("id") public id?: string;

  public settings!: Settings[];
  public subscriptions: Subscription[] = [];
  public loaded = false;

  public isExpanded: { [key: number]: boolean } = {};

  // Parameter templates
  @ViewChild("numberTmpl", { static: true })
  public numberTmpl!: TemplateRef<NumberParameter>;

  @ViewChild("colorRangeTmpl", { static: true })
  public colorRangeTmpl!: TemplateRef<ColorRangeParameter>;

  @ViewChild("booleanTmpl", { static: true })
  public booleanTmpl!: TemplateRef<BooleanParameter>;

  @ViewChild("enumTmpl", { static: true })
  public enumTmpl!: TemplateRef<EnumParameter>;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.getSettings();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes["id"]) {
      this.getSettings();
    }
  }

  getSettings() {
    if (!this.id) {
      return;
    }

    const sub = this.api
      .getSettings(this.id)
      .subscribe(
        settings => {
          this.settings = settings;
          this.settings.forEach((_s, i) => this.isExpanded[i] = true);
          this.loaded = true;
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

  toggleExpansion(i: number) {
    this.isExpanded[i] = !this.isExpanded[i];
  }
}
