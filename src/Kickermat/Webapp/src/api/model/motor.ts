import { Entity } from './base';
import { Peripheral } from '.';

export enum MotorFunction {
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
  function: MotorFunction;
  bar: Bar;
  canId: number;
  nmtState: NmtState;
  operatingState: OperatingState;
  operatingMode: OperatingMode;
  error: string | undefined;
}

export interface Diagnostics extends Peripheral {
  motors: Motor[];
}
