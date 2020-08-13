export interface NavItem {
  title: string;
  open: boolean;
  selected: boolean;
  disabled: boolean;
  level: number;
  path: string | any[];
  queryParams: { [k: string]: any },
  icon?: string;
  children?: NavItem[]
}

export const KICKER_NAV_ITEMS: NavItem[] = [
  {
    path: 'kicker',
    queryParams: {},
    level: 1,
    title: 'Kicker',
    icon: 'play-circle',
    open: false,
    selected: false,
    disabled: false
  },
  {
    path: 'motor',
    queryParams: {},
    level: 1,
    title: 'Motors',
    icon: 'thunderbolt',
    open: false,
    selected: false,
    disabled: false
  },
  {
    path: [],
    queryParams: {},
    level: 1,
    title: 'Cameras',
    icon: 'video-camera',
    open: false,
    selected: false,
    disabled: false,
  },
]

