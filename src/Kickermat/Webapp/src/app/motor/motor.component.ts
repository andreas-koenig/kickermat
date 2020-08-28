import { Component, ElementRef, ViewChild, OnInit, OnDestroy } from '@angular/core';

import { Subscription } from 'rxjs';

import { Motor, MotorFunction, Diagnostics, Peripheral } from '@api/model';
import { ApiService } from '@api/api.service';
import { barToString } from './names';

type Status = 'loading' | 'success' | 'error';
type Selection = 'info' | 'nmt' | 'operation';

@Component({
  selector: 'oth-motor',
  templateUrl: './motor.component.html',
  styleUrls: ['./motor.component.scss']
})
export class MotorComponent implements OnInit, OnDestroy {
  public status: Status = 'loading';
  public diagnostics: Diagnostics | undefined;
  public selectedMotor: Motor | undefined;
  public selectedItem: Selection = 'info';
  public motorPeripheral: Peripheral | undefined;

  @ViewChild('info', { static: true }) public infoTemplate!: ElementRef<HTMLElement>;
  @ViewChild('nmt', { static: true }) public nmtTemplate!: ElementRef<HTMLElement>;
  @ViewChild('operation', { static: true }) public operationTemplate!: ElementRef<HTMLElement>;

  public barToString = barToString;
  public functionEnum = MotorFunction;

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    const sub = this.api.getMotorDiagnostics()
      .subscribe(diagnostics => {
        diagnostics.motors = diagnostics.motors.sort((m1, m2): number =>
          (m1.bar === m2.bar) ? m2.function - m1.function : m1.bar - m2.bar
        );
        this.diagnostics = diagnostics;
        this.selectedMotor = this.diagnostics.motors[0];
        this.status = 'success';
      });
    
    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  public changeItem(item: Selection): void {
    this.selectedItem = item;
  }

  public changeMotor(motor: Motor): void {
    this.selectedMotor = motor;
  }

  public getInfoTemplate(): ElementRef {
    switch (this.selectedItem) {
      case 'info': return this.infoTemplate;
      case 'nmt': return this.nmtTemplate;
      case 'operation': return this.operationTemplate;
    }
  }
}
