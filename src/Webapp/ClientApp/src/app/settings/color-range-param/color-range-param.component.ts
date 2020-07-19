import { Component, Input, OnInit } from '@angular/core';
import { HsvColor, ColorRange, ColorRangeParameter } from '@api/api.model';
import { NzMarks } from 'ng-zorro-antd';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { ApiService } from '../../../api/api.service';
import { Subscription } from 'rxjs';

interface RgbColor {
  red: number;
  green: number;
  blue: number;
}

@Component({
  selector: 'app-color-range-param',
  templateUrl: './color-range-param.component.html',
  styleUrls: ['./color-range-param.component.scss']
})
export class ColorRangeParamComponent implements OnInit {
  @Input("param") public param!: ColorRangeParameter;
  @Input("settings") public settings!: string;

  private subs: Subscription[] = [];
  public isUpdating = false;

  public hueRange: number[] = []
  public satRange: number[] = []
  public valRange: number[] = []

  public degreeMarks: NzMarks = {
    0: '0°',
    360: '360°'
  }

  public percentMarks: NzMarks = {
    0: '0%',
    100: '100%'
  }

  constructor(
    private domSanitizer: DomSanitizer,
    private api: ApiService) {}

  ngOnInit() {
    this.updateModel(this.param.value);
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  protected updateModel(value: ColorRange) {
    const l = value.lower;
    const u = value.upper;

    this.hueRange = [l.hue, u.hue];
    this.satRange = [l.saturation, u.saturation];
    this.valRange = [l.value, u.value];

    this.isUpdating = false;
  }

  private getColorRange(): ColorRange {
    return {
      lower: {
        hue: this.hueRange[0],
        saturation: this.satRange[0],
        value: this.valRange[0]
      },
      upper: {
        hue: this.hueRange[1],
        saturation: this.satRange[1],
        value: this.valRange[1]
      }
    }
  }

  public updateColorRange() {
    this.isUpdating = true;

    const sub = this.api.updateParam<ColorRangeParameter>(
      this.settings,
      this.param.name,
      this.getColorRange(),
      this.updateModel.bind(this),
      this.updateModel.bind(this));

    this.subs.push(sub);
  }

  public getGradient(): SafeStyle {
    const range = this.getColorRange();
    const lower = this.hsvToRgb(range.lower);
    const upper = this.hsvToRgb(range.upper);

    const gradient = `linear-gradient(90deg, rgb(${lower.red}, ${lower.green}, ${lower.blue}) 0%, rgb(${upper.red}, ${upper.green}, ${upper.blue}) 100%)`;

    return this.domSanitizer.bypassSecurityTrustStyle(gradient);
  }

  public hsvToRgb(hsv: HsvColor): RgbColor {
    const h = hsv.hue // [0, 360]
    const s = hsv.saturation / 100; // [0, 1]
    const v = hsv.value / 100; // [0, 1]

    let max = function(a: number, b: number): number {
      return a > b ? a : b;
    }

    let min = function (a: number, b: number): number {
      return a < b ? a : b;
    }

    let f = function(n: number): number {
      const k = (n + h / 60) % 6;
      return v - v * s * max(min(k, min(4 - k, 1)), 0);
    }

    // r, g, b in range [0, 1]
    return {
      red: f(5) * 255,
      green: f(3) * 255,
      blue: f(1) * 255
    };
  }
}
