import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { HUB_CALIBRATION, HUB_CALIBRATION_START } from '../../api/api';

@Component({
  selector: 'app-calibration',
  templateUrl: './calibration.component.html',
  styleUrls: ['./calibration.component.scss']
})
export class CalibrationComponent implements OnInit {
  private connection: HubConnection;

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl(HUB_CALIBRATION)
      .configureLogging(LogLevel.Debug)
      .build();
  }

  ngOnInit() {
    console.log('Calibration OnInit');
  }

  public startCalibration() {
    console.log("[CameraCalibration] Calibration started");

    this.connection.start().catch(err => {
      console.log('[CameraCalibration] Error while starting connection!');
      console.log(err);
    }).then(() => {
      console.log('[CameraCalibration] Started connection')
      this.startCalibrationStream();
    });
  }

  private startCalibrationStream() {
    this.connection.stream(HUB_CALIBRATION_START).subscribe({
      next: (count: number) => {
        console.log(count);
      },
      complete: () => {
        console.log('[CameraCalibration] Calibration completed');
        // no action
      },
      error: (err: any) => {
        console.log('[CameraCalibration] Calibration failed');
        console.log(err);
      }
    });
  }
}
