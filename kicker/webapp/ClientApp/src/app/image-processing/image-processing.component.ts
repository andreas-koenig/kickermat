import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../api/api.service';

@Component({
  selector: 'app-image-processing',
  templateUrl: './image-processing.component.html',
  styleUrls: ['./image-processing.component.scss']
})
export class ImageProcessingComponent implements OnInit {

  constructor(private api: ApiService) { }

  ngOnInit() {
  }

  public start() {
    this.api.startBallSearch();
  }
}
