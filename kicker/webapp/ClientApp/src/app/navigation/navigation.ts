import { Route } from '@angular/router';
import { CameraComponent } from '../camera/camera.component';
import { CalibrationComponent } from '../calibration/calibration.component';

export interface NavItem extends Route {
  title: string;
  open: boolean;
  selected: boolean;
  disabled: boolean;
  level: number;
  icon?: string;
  children?: NavItem[]
}

export const KICKER_NAV_ITEMS: NavItem[] = [
  {
    path: 'camera',
    component: CameraComponent,
    level: 1,
    title: 'Camera',
    icon: 'camera',
    open: false,
    selected: false,
    disabled: false,
    children: [
      {
        path: 'calibration',
        component: CalibrationComponent,
        level: 2,
        title: 'Calibration',
        icon: 'sliders',
        open: false,
        selected: false,
        disabled: false
      }
    ]
  },
]
