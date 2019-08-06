import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { KICKER_NAV_ITEMS } from './navigation/navigation';
import { GameComponent } from './game/game.component';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { CameraComponent } from './camera/camera.component';
import { CalibrationComponent } from './calibration/calibration.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    GameComponent,
    VideoPlayerComponent,
    CameraComponent,
    CalibrationComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(KICKER_NAV_ITEMS),
    FlexLayoutModule,
    NgZorroAntdModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent],
})
export class AppModule { }
