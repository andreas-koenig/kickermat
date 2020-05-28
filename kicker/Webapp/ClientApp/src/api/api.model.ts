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

export enum Function {
  Rotation = 0,
  Shift = 1
}

export enum Bar {
  Keeper = 0,
  Defense = 1,
  Midfield = 2,
  Striker = 3
}

export enum NmtState {
  Initialization = 0,
  PreOperational = 1,
  Operational = 2,
  Stopped = 3
}

export enum OperatingState {
  NotReadyToSwitchOn = 0,
  SwitchOnDisabled = 1,
  ReadyToSwitchOn = 2,
  SwitchedOn = 3,
  OperationEnable = 4,
  QuickStopActive = 5,
  FaultReactionActive = 6,
  Fault = 7
}

export enum OperatingMode {
  ProfilePositionMode = 0,  // Punkt zu Punkt
  ProfileVelocityMode = 1,  // Geschwindigkeitsprofil
  HomingMode = 2,           // Referenzierung
  OscillatorMode = 3,       // Drehzahlregelung
  GearingMode = 4,          // Elektronisches Getriebe
  JogMode = 5               // Manuellfahrt
}

export interface Motor {
  model: string;
  function: Function;
  bar: Bar;
  canId: number;
  nmtState: NmtState;
  operatingState: OperatingState;
  operatingMode: OperatingMode;
  error: string | undefined;
}
