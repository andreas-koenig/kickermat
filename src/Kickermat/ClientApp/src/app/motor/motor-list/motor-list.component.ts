import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Motor, Function } from '@api/api.model';

import { barToString } from '../names';

@Component({
  selector: 'app-motor-list',
  templateUrl: './motor-list.component.html',
  styleUrls: ['./motor-list.component.scss'],
})
export class MotorListComponent implements OnInit {
  @Input('motors') public motors!: Motor[];
  @Input() public selected!: Motor;
  @Output() public selectedChange: EventEmitter<Motor> = new EventEmitter<Motor>();

  public functionEnum = Function;
  public barToString = barToString;

  constructor() { }

  ngOnInit() {
  }

  public selectMotor(motor: Motor) {
    this.selectedChange.emit({ ...motor });
  }
}

