import { Component, OnInit } from '@angular/core';
import { KICKER_NAV_ITEMS, NavItem } from './navigation';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  public menus: NavItem[] = [];

  public isCollapsed = false;
  
  public constructor() {
    this.menus = KICKER_NAV_ITEMS;
  }

  ngOnInit() { }

  public toggleCollapsed() {
    this.isCollapsed = !this.isCollapsed;
  }
}
