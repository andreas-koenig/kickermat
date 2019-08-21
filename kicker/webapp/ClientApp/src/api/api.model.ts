export enum VideoSource {
  CAMERA = 0,
  CAMERA_CALIBRATION = 1
}

export enum KickerComponent {
  Camera = "Camera",
}

export enum KickerDataType {
  Number = 0,
  String = 1,
  Color = 2
}

export interface KickerParameter {
  name: string;
  description: string;
  dataType: KickerDataType;
  value: object;
  defaultValue: object;
  min: object;
  max: object;
}
