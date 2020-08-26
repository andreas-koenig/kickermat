import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

// Carbon Module
import { CarbonModule } from './carbon.module';

// Kickermat Modules
import { AppRoutingModule } from './app-routing.module';
import { SettingsModule } from './settings/settings.module';
import { MotorModule } from './motor/motor.module';
import { PageModule } from './page/page.module';

// Kickermat Components
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { GameComponent } from './game/game.component';
import { UserInterfaceComponent } from './user-interface/user-interface.component';
import { VideoInterfaceComponent } from './user-interface/video-interface/video-interface.component';
import { CameraComponent } from './camera/camera.component';
import { PlayersListComponent } from './game/players-list/players-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    GameComponent,
    UserInterfaceComponent,
    CameraComponent,
    PlayersListComponent,
    VideoInterfaceComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    SettingsModule,
    MotorModule,
    PageModule,
    CarbonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
