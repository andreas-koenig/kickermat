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
import { GameComponent } from './game/game.component';
import { CameraComponent } from './camera/camera.component';
import { CalibrationComponent } from './calibration/calibration.component';
import { ParameterListComponent } from './parameter-list/parameter-list.component';
import { KickerComponent } from './kicker/kicker.component';
import { ImageProcessingComponent } from './image-processing/image-processing.component';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { NumberParameterComponent } from './parameter-list/number-parameter/number-parameter.component';
import { ColorRangeParameterComponent } from './parameter-list/color-range-parameter/color-range-parameter.component';
import { MotorComponent } from './motor/motor.component';
import { NmtStateComponent } from './motor/nmt-state/nmt-state.component';
import { MotorOverviewComponent } from './motor/motor-overview/motor-overview.component';
import { InfoTableComponent } from './motor/info-table/info-table.component';
import { OperatingStateComponent } from './motor/operating-state/operating-state.component';
import { KickermatComponent } from './motor/kickermat/kickermat.component';

const routes: Route[] = [
  {
    path: '',
    redirectTo: 'kicker',
    pathMatch: 'full'
  },
  {
    path: 'kicker',
    component: KickerComponent
  },
  {
    path: 'motor',
    component: MotorComponent
  },
  {
    path: 'camera/settings',
    component: CameraComponent,
  },
  {
    path: 'camera/calibration',
    component: CalibrationComponent
  },
  {
    path: 'imgproc',
    component: ImageProcessingComponent
  }
]

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    GameComponent,
    VideoPlayerComponent,
    CameraComponent,
    CalibrationComponent,
    ParameterListComponent,
    KickerComponent,
    ImageProcessingComponent,
    VideoPlayerComponent,
    NumberParameterComponent,
    ColorRangeParameterComponent,
    MotorComponent,
    NmtStateComponent,
    MotorOverviewComponent,
    InfoTableComponent,
    OperatingStateComponent,
    KickermatComponent,
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
})
export class AppModule {}
