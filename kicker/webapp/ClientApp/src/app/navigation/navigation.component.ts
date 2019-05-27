import { Component, OnInit } from '@angular/core';
import { KickerNavigation, NavItem } from './navigation';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  public navItems: NavItem[] = [];

  public menus = [
    {
      level: 1,
      title: 'Game',
      icon: 'caret-right',
      open: false,
      selected: false,
      disabled: false
    },
    {
      level: 1,
      title: 'Camera',
      icon: 'camera',
      open: false,
      selected: false,
      disabled: false
    },
    {
      level: 1,
      title: 'Image Processing',
      icon: 'picture',
      open: false,
      selected: false,
      disabled: false
    },
    {
      level: 1,
      title: 'Settings',
      icon: 'setting',
      open: false,
      selected: false,
      disabled: false
    }
  ];

  public isCollapsed = false;

  ngOnInit() {
    this.navItems = KickerNavigation.navItems;
  }

  public toggleCollapsed() {
    this.isCollapsed = !this.isCollapsed;
  }
}
