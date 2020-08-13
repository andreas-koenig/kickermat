import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';

import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { CameraComponent } from './camera/camera.component';
import { CalibrationComponent } from './camera/calibration/calibration.component';
import { PlayerComponent } from './player/player.component';
import { MotorComponent } from './motor/motor.component';
import { NmtStateComponent } from './motor/nmt-state/nmt-state.component';
import { InfoTableComponent } from './motor/info-table/info-table.component';
import { OperatingStateComponent } from './motor/operating-state/operating-state.component';
import { KickerComponent } from './motor/kicker/kicker.component';
import { OperatingModeComponent } from './motor/operating-mode/operating-mode.component';
import { MotorListComponent } from './motor/motor-list/motor-list.component';
import { KickermatComponent } from './kickermat/kickermat.component';

import { SettingsComponent } from './settings/settings.component';
import { NumberParamComponent } from './settings/number-param/number-param.component';
import { ColorRangeParamComponent } from './settings/color-range-param/color-range-param.component';
import { BooleanParameterComponent } from './settings/boolean-param/boolean-param.component';
import { EnumParamComponent } from './settings/enum-param/enum-param.component';
import { UserInterfaceComponent } from './user-interface/user-interface.component';
import { VideoInterfaceComponent } from './user-interface/video-interface/video-interface.component';


const routes: Route[] = [
  {
    path: '',
    redirectTo: 'kicker',
    pathMatch: 'full'
  },
  {
    path: 'kicker',
    component: KickermatComponent
  },
  {
    path: 'motor',
    component: MotorComponent
  },
  {
    path: 'camera',
    component: CameraComponent,
  },
  {
    path: 'camera/calibration',
    component: CalibrationComponent
  },
]

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    CameraComponent,
    CalibrationComponent,
    PlayerComponent,
    MotorComponent,
    NmtStateComponent,
    InfoTableComponent,
    OperatingStateComponent,
    KickermatComponent,
    KickerComponent,
    OperatingModeComponent,
    MotorListComponent,
    SettingsComponent,
    NumberParamComponent,
    ColorRangeParamComponent,
    BooleanParameterComponent,
    EnumParamComponent,
    UserInterfaceComponent,
    VideoInterfaceComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    FlexLayoutModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgZorroAntdModule
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent],
  entryComponents: [SettingsComponent],
})
export class AppModule {}
