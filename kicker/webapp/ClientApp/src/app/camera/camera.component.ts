import { Component, OnInit } from '@angular/core';
import { KickerComponent } from '../../api/api.model';

@Component({
  selector: 'app-camera',
  templateUrl: './camera.component.html',
  styleUrls: ['./camera.component.scss']
})
export class CameraComponent implements OnInit {
  public kickerComponentEnum = KickerComponent;

  constructor() { }

  ngOnInit() {
  }

}
