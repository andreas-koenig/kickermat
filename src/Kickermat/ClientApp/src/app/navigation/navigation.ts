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
    icon: 'play-circle',
    open: false,
    selected: false,
    disabled: false
  },
  {
    path: 'motor',
    level: 1,
    title: 'Motors',
    icon: 'thunderbolt',
    open: false,
    selected: false,
    disabled: false
  },
  {
    path: 'camera',
    level: 1,
    title: 'Cameras',
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
]
