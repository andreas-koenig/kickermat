// Settings
export interface KickerParameter<T> {
  name: string;
  description: string;
  value: T;
  defaultValue: T;
}

export interface NumberParameter extends KickerParameter<number> {
  min: number;
  max: number;
  step: number;
}

export interface ColorRangeParameter extends KickerParameter<ColorRange> {}

export interface ColorRange {
  lower: HsvColor;
  upper: HsvColor;
}

export interface HsvColor {
  hue: number; // [0, 360]
  saturation: number; // [0, 100]
  value: number; // [0, 100]
}

export interface BooleanParameter extends KickerParameter<boolean> {}

export interface EnumParameter extends KickerParameter<number> {
  options: { key: number, value: string }[];
}

export interface UpdateSettingsResponse<T extends KickerParameter<T>> {
  message: string;
  value: T;
}

export interface ParameterUpdate {
  settings: string;
  parameter: string;
  value: any;
}

// Game
export interface Game {
  state: GameState;
  player?: KickermatPlayer;
}

export enum GameState {
  NoGame = 0,
  Running = 1,
  Paused = 2,
}

// Players
export interface KickermatPlayer {
  name: string;
  description: string;
  authors: string[];
  emoji: string;
}

export interface Settings {
  name: string;
  parameters: KickerParameter<any>[];
}

// User Interface
export enum UserInterface {
  Video = 0,
}

export interface VideoChannel {
  name: string;
  description: string;
}

export interface ChannelsResponse {
  channels: VideoChannel[];
  currentChannel: VideoChannel;
}

// Motor Diagnostics
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
