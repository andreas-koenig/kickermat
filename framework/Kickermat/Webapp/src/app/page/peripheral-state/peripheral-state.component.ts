import { Component, Input } from '@angular/core';

import { Peripheral, PeripheralState } from '@api/model';

@Component({
  selector: 'oth-peripheral-state',
  template: `
    <div class="state" *ngIf="peripheral">
      <div class="dot" [class]="class()"></div>
      <div class="text">{{text()}}</div>
    </div>
  `,
  styleUrls: [ 'peripheral-state.component.scss' ],
})
export class PeripheralStateComponent {
  @Input() public peripheral: Peripheral;

  text(): string {
    switch (this.peripheral.peripheralState) {
      case PeripheralState.DriversNotInstalled:
        return "No drivers installed";
      case PeripheralState.NotConnected:
        return "Not connected";
      case PeripheralState.Initializing:
        return "Initializing";
      case PeripheralState.Error:
        return "Error";
      case PeripheralState.Ready:
        return "Ready";
    }
  }

  class(): string {
    switch(this.peripheral.peripheralState) {
      case PeripheralState.DriversNotInstalled:
        return "drivers-not-installed";
      case PeripheralState.NotConnected:
        return "not-connected";
      case PeripheralState.Initializing:
        return "initializing";
      case PeripheralState.Error:
        return "error";
      case PeripheralState.Ready:
        return "ready";
    }
  }
}
