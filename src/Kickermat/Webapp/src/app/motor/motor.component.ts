import { Component, ElementRef, ViewChild, OnInit, OnDestroy } from '@angular/core';

import { Subscription } from 'rxjs';

import { Motor, MotorFunction } from '@api/api.model';
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
  public subscription$: Subscription | undefined;
  public motors: Motor[] = [];
  public selectedMotor: Motor | undefined;
  public selectedItem: Selection = 'info';

  @ViewChild('info', { static: true }) public infoTemplate!: ElementRef<HTMLElement>;
  @ViewChild('nmt', { static: true }) public nmtTemplate!: ElementRef<HTMLElement>;
  @ViewChild('operation', { static: true }) public operationTemplate!: ElementRef<HTMLElement>;

  public barToString = barToString;
  public functionEnum = MotorFunction;

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.subscription$ = this.api.getMotorDiagnostics()
      .subscribe(motors => {
        this.motors = motors.sort((m1, m2): number =>
          (m1.bar === m2.bar) ? m2.function - m1.function : m1.bar - m2.bar
        );
        this.selectedMotor = { ...this.motors[0] };
        this.status = 'success';
      });
  }

  ngOnDestroy(): void {
    if (this.subscription$) {
      this.subscription$.unsubscribe();
    }
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
