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

enum NmtState {
  Initialization = 0,
  PreOperational = 1,
  Operational = 2,
  Stopped = 3
}

enum OperatingState {
  NotReadyToSwitchOn = 0,
  SwitchOnDisabled = 1,
  ReadyToSwitchOn = 2,
  SwitchedOn = 3,
  OperationEnable = 4,
  QuickStopActive = 5,
  FaultReactionActive = 6,
  Fault = 7
}

enum OperatingMode {
  HomingMode = 0,
  ProfilePositionMode = 1,
  ProfileVelocityMode = 2,
  CurrentControl = 3,
  RotationSpeedControl = 4,
  ElectricTransmission = 5,
  Manual = 6
}

export interface Motor {
  model: string;
  function: string;
  bar: string;
  canOpenId: number;
  nmtState: NmtState;
  error: string;
  operatingState: OperatingState;
  operatingMode: OperatingMode;
}
