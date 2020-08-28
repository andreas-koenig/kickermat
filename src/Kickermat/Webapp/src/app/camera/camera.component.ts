import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '@api/api.service';
import { Camera } from '@api/model';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'oth-camera',
  templateUrl: './camera.component.html',
  styleUrls: [ './camera.component.scss' ],
})
export class CameraComponent implements OnInit, OnDestroy {
  public camera?: Camera;
  public isLoading = true;

  private subs: Subscription[] = [];

  constructor(
    private api: ApiService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const sub = this.route
      .queryParams
      .subscribe(params => this.loadCamera(params.id));

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  loadCamera(cameraName: string): void {
    const sub = this.api.getCamera(cameraName).subscribe(
      (cam) => {
        this.camera = cam;
        this.isLoading = false;
      });

    this.subs.push(sub);
  }
}

