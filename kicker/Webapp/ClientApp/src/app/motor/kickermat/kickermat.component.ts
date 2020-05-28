import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Motor, Function, Bar } from '@api/api.model';
import { barToString } from '../names';

@Component({
  selector: 'app-kickermat',
  templateUrl: './kickermat.component.html',
  styleUrls: ['./kickermat.component.scss']
})
export class KickermatComponent {
  @Input('motors') public motors!: Motor[];
  @Input() public selected!: Motor;
  @Output() public selectedChange: EventEmitter<Motor> = new EventEmitter<Motor>();

  public functionEnum = Function;
  public barEnum = Bar;
  public barToString = barToString;

  public isSelected(motor: Motor, f: Function, b: Bar): boolean {
    return (motor.function === f) && (motor.bar === b);
  }

  public selectMotor(f: Function, b: Bar): void {
    const filtered = this.motors.filter(val => val.function === f && val.bar === b);
    if (filtered.length === 1) {
      this.selectedChange.emit({ ...filtered[0] });
    }
  }
}
