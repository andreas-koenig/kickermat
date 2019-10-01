import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { GameComponent } from './game/game.component';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { CameraComponent } from './camera/camera.component';
import { CalibrationComponent } from './calibration/calibration.component';
import { ParameterListComponent } from './parameter-list/parameter-list.component';
import { KickerParameterComponent } from './parameter-list/kicker-parameter/kicker-parameter.component';
import { KickerComponent } from './kicker/kicker.component';
import { ImageProcessingComponent } from './image-processing/image-processing.component';
import { VideoPlayerComponent } from './video-player/video-player.component';

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
    KickerParameterComponent,
    KickerComponent,
    ImageProcessingComponent,
    VideoPlayerComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    FlexLayoutModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgZorroAntdModule,
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent],
})
export class AppModule { }
