export interface NavItem {
  title: string;
  open: boolean;
  selected: boolean;
  disabled: boolean;
  level: number;
  path: string;
  icon?: string;
  children?: NavItem[]
}

export const KICKER_NAV_ITEMS: NavItem[] = [
  {
    path: 'kicker',
    level: 1,
    title: 'Kicker',
    icon: 'play-square',
    open: false,
    selected: false,
    disabled: false
  },
  {
    path: 'camera',
    level: 1,
    title: 'Camera',
    icon: 'video-camera',
    open: false,
    selected: false,
    disabled: false,
    children: [
      {
        path: 'settings',
        level: 2,
        title: 'Settings',
        icon: 'setting',
        open: false,
        selected: false,
        disabled: false
      },
      {
        path: 'calibration',
        level: 2,
        title: 'Calibration',
        icon: 'sliders',
        open: false,
        selected: false,
        disabled: false
      }
    ]
  },
  {
    path: 'image',
    level: 1,
    title: 'Image Processing',
    icon: 'picture',
    open: false,
    selected: false,
    disabled: false,
    children: [
      {
        path: 'preprocessing',
        level: 2,
        title: 'Preprocessing',
        icon: 'scan',
        open: false,
        selected: false,
        disabled: false
      }
    ]
  }
]
