export enum VideoSource {
  CAMERA = 0,
  CAMERA_CALIBRATION = 1
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
