import { Component, OnInit, OnDestroy } from '@angular/core';
import { KICKER_NAV_ITEMS, NavItem } from './navigation';
import { ApiService } from '@api/api.service';
import { Camera } from '@api/api.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit, OnDestroy {
  public menus: NavItem[] = [];
  private subs: Subscription[] = [];

  public isCollapsed = false;
  
  public constructor(private api: ApiService) {
    this.menus = KICKER_NAV_ITEMS;
  }

  ngOnInit() {
    const sub = this.api.getCameras().subscribe(
      (resp) => this.createCameraMenu(resp)
    );

    this.subs.push(sub);
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  public createCameraMenu(cameras: Camera[]) {
    const cameraMenuItem = this.menus.filter(item => item.title === "Cameras")[0];

    const cameraItems: NavItem[] = cameras.map(cam => ({
      path: "camera",
      queryParams: {"name": cam.name},
      title: cam.name,
      level: 2,
      open: false,
      selected: false,
      disabled: false,
    }));

    cameraMenuItem.children = cameraItems;
  }

  public toggleCollapsed() {
    this.isCollapsed = !this.isCollapsed;
  }
}
