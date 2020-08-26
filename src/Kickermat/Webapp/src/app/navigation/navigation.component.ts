import { Component, OnInit, HostBinding, Input, OnDestroy } from '@angular/core';
import { NavigationExtras } from '@angular/router';

import { Subscription } from 'rxjs';

import { ApiService } from '@api/api.service';
import { Camera } from '@api/api.model';

@Component({
  selector: 'oth-navigation',
  templateUrl: './navigation.component.html',
})
export class NavigationComponent implements OnInit, OnDestroy {
  @Input('organization') public organization: string;
  @Input('application') public application: string;

  @HostBinding('class.bx--header') headerClass = true;

  public cameras: Camera[] = [];
  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    const sub = this.api.getCameras()
      .subscribe(
        cameras => this.cameras = cameras,
        error => console.log(`Failed to query cameras: ${error}`)
      );

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  public getRouteExtras(camera: Camera): NavigationExtras {
    return { queryParams: { name: camera.name } };
  }
}
