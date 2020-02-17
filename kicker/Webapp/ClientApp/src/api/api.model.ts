export enum VideoSource {
  Camera = "Camera",
  Calibration = "Calibration",
  ImageProcessing = "ImageProcessing",
}

export interface Channel {
  id: string;
  name: string;
  description: string;
}

export enum KickerComponent {
  Camera = "Camera",
}

export interface KickerParameter {
  name: string;
  description: string;
  value: object | number;
}

export interface NumberParameter extends KickerParameter {
  defaultValue: number;
  min: number;
  max: number;
  step: number;
}

export interface ColorRangeParameter extends KickerParameter {
  defaultValue: ColorRange;
  value: ColorRange;
}

export interface ColorRange {
  lower: HsvColor;
  upper: HsvColor;
}

export interface HsvColor {
  hue: number; // [0, 360]
  saturation: number; // [0, 100]
  value: number; // [0, 100]
}
