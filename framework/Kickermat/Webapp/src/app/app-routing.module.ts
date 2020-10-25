import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GameComponent } from '@app/game/game.component';
import { MotorComponent } from './motor/motor.component';
import { CameraComponent } from './camera/camera.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'game',
    pathMatch: 'prefix',
  },
  {
    path: 'game',
    component: GameComponent,
  },
  {
    path: 'motor',
    component: MotorComponent,
  },
  {
    path: 'camera',
    component: CameraComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
