import { Entity, Named } from './base';

export interface Peripheral extends Entity, Named {
  peripheralState: PeripheralState;
}

export enum PeripheralState {
  DriversNotInstalled = 0,
  NotConnected = 1,
  Initializing = 2,
  Error = 3,
  Ready = 4,
}
