import { Component, ElementRef, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Motor } from '@api/api.model';
import { Selection } from './info-table/info-table.component';
import { Subscription } from 'rxjs';
import { ApiService } from '@api/api.service';
import { barToString } from './names';

@Component({
  selector: 'app-motor',
  templateUrl: './motor.component.html',
  styleUrls: ['./motor.component.scss']
})
export class MotorComponent implements OnInit, OnDestroy {
  public loading = true;
  public subscription$: Subscription | undefined;
  public motors: Motor[] = [];
  public selectedMotor: Motor | undefined;
  public selectedItem: Selection | undefined;

  @ViewChild('bar', { static: true }) public barTemplate!: ElementRef<HTMLElement>;
  @ViewChild('canId', { static: true }) public canOpenIdTemplate!: ElementRef<HTMLElement>;
  @ViewChild('nmtState', { static: true }) public nmtStateTemplate!: ElementRef<HTMLElement>;
  @ViewChild('operatingState', { static: true }) public operatingStateTemplate!: ElementRef<HTMLElement>;
  @ViewChild('operatingMode', { static: true }) public operatingModeTemplate!: ElementRef<HTMLElement>;
  @ViewChild('error', { static: true }) public errorTemplate!: ElementRef<HTMLElement>;

  public barToString = barToString;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.subscription$ = this.api.getMotorDiagnostics()
      .subscribe(motors => {
        this.motors = motors.sort((m1, m2): number =>
          (m1.bar === m2.bar) ? m2.function - m1.function : m1.bar - m2.bar
        );
        this.selectedMotor = { ...this.motors[0] };
        this.loading = false;
      });
  }

  ngOnDestroy() {
    this.subscription$ && this.subscription$.unsubscribe();
  }

  public changeItem(item: Selection) {
    this.selectedItem = item;
  }

  public getInfoTemplate(item: Selection): ElementRef {
    switch (item) {
      case "bar": return this.barTemplate;
      case "canId": return this.canOpenIdTemplate;
      case "nmtState": return this.nmtStateTemplate;
      case "operatingState": return this.operatingStateTemplate;
      case "operatingMode": return this.operatingModeTemplate;
      case "error": return this.errorTemplate;
    }
  }
}
