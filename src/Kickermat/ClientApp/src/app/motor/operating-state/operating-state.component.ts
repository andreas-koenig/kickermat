import { Component, Input } from '@angular/core';
import { OperatingState, Motor } from '@api/api.model';
import { opStateToString } from '../names';

@Component({
  selector: 'app-operating-state',
  templateUrl: './operating-state.component.html',
  styleUrls: ['../state-diagram.scss']
})
export class OperatingStateComponent {
  @Input('motor') public motor!: Motor;

  public operatingStateEnum = OperatingState;
  public opStateToString = opStateToString;
}
