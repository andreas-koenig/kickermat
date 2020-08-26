import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Motor, MotorFunction, Bar } from '@api/api.model';
import { barToString } from '../names';

@Component({
  selector: 'oth-motor-info',
  templateUrl: './motor-info.component.html',
  styleUrls: [
    './motor-info.component.scss',
    '../motor-styles.scss',
  ]
})
export class MotorInfoComponent {
  @Input('motors') public motors!: Motor[];
  @Input() public selected!: Motor;
  @Output() public selectedChange: EventEmitter<Motor> = new EventEmitter<Motor>();

  public functionEnum = MotorFunction;
  public barEnum = Bar;
  public barToString = barToString;

  public isSelected(motor: Motor, f: MotorFunction, b: Bar): boolean {
    return (motor.function === f) && (motor.bar === b);
  }

  public selectMotor(f: MotorFunction, b: Bar): void {
    const filtered = this.motors.filter(val => val.function === f && val.bar === b);
    if (filtered.length === 1) {
      this.selectedChange.emit({ ...filtered[0] });
    }
  }
}
