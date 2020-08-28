import { Component, OnInit, Input } from '@angular/core';
import { NmtState } from '@api/model';

@Component({
  selector: 'oth-nmt-state',
  templateUrl: './nmt-state.component.html',
  styleUrls: ['../motor-styles.scss']
})
export class NmtStateComponent implements OnInit {
  @Input('nmtState') public nmtState!: NmtState;

  public tooltipInitialization = 'The motor just received power and started its initialization';
  public tooltipPreOperational = 'The motor has finished the initialization and broadcast the\
    NMT boot-up message over the network.';
  public tooltipOperational = 'The motor is fully operational and all communication objects\
    defined by CANopen are enabled.';
  public tooltipStopped = 'The motor stopped due to a failure.';

  public nmtStateEnum = NmtState;

  constructor() { }

  ngOnInit(): void {
  }
}
