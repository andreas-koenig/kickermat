import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// Carbon Module
import { CarbonModule } from '@app/carbon.module';

// Kickermat modules
import { PageModule } from '@app/page/page.module';

// Motor components
import { MotorComponent } from './motor.component';
import { OperationComponent } from './operation/operation.component';
import { NmtStateComponent } from './nmt-state/nmt-state.component';
import { MotorInfoComponent } from './motor-info/motor-info.component';

@NgModule({
  declarations: [
    MotorComponent,
    OperationComponent,
    NmtStateComponent,
    MotorInfoComponent,
  ],
  imports: [
    BrowserModule,
    PageModule,
    CarbonModule
  ]
})
export class MotorModule { }
