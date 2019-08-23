import { Component, OnInit } from '@angular/core';
import { VideoSource } from '@api/api.model';

@Component({
  selector: 'app-preprocessing',
  templateUrl: './preprocessing.component.html',
  styleUrls: ['./preprocessing.component.scss']
})
export class PreprocessingComponent implements OnInit {
  public videoSource = VideoSource.Preprocessing;

  constructor() { }

  ngOnInit() {
  }
}
