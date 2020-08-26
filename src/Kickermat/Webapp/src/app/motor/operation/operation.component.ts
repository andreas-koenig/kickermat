import { Component, Input } from '@angular/core';
import { OperatingState, Motor, OperatingMode } from '@api/api.model';
import { opStateToString, opModeToString } from '../names';

@Component({
  selector: 'oth-operation',
  templateUrl: './operation.component.html',
  styleUrls: ['../motor-styles.scss'],
})
export class OperationComponent {
  @Input('motor') public motor!: Motor;

  public operatingStateEnum = OperatingState;
  public opStateToString = opStateToString;
  public opModeToString = opModeToString;

  getOpModeDescription(): string {
    switch (this.motor.operatingMode) {
      case OperatingMode.ProfilePositionMode:
        return 'In this mode a movement is executed in a point-to-point manner from a\
          starting to a target position. The target position can be specified either\
          relatively or absolutely with respect to the zero point of the axis.';

      case OperatingMode.ProfileVelocityMode:
        return 'In this mode the motor is accelerated to an adjustable target velocity.';

      case OperatingMode.HomingMode:
        return 'This mode is used to establish an absolute dimensional reference of\
          the motor position to a defined axis position. Thereby a zero point, which is\
          required to move to an absolute point on the axis, can be defined. This mode\
          is used for during the process of calibration the motors.';

      case OperatingMode.GearingMode:
      case OperatingMode.JogMode:
      case OperatingMode.OscillatorMode:
      default:
        return '';
    }
  }
}
