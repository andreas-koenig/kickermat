import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-calibration',
  templateUrl: './calibration.component.html',
  styleUrls: ['./calibration.component.scss']
})
export class CalibrationComponent implements OnInit {

  constructor() {
    console.log('Calibration Constructor');
  }

  ngOnInit() {
    console.log('Calibration OnInit');
  }

}
