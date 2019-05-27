import { Route } from '@angular/router';

export abstract class KickerNavigation {
  public static navItems: NavItem[] = []
}

export interface NavItem extends Route {
  name: string;
  icon?: string;
}

export function NavItem(navItem: NavItem) {
  KickerNavigation.navItems.push(navItem);

  return function (_target: any) { };
}

