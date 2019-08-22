import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { HUB_CALIBRATION, HUB_CALIBRATION_START } from '../../api/api';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-calibration',
  templateUrl: './calibration.component.html',
  styleUrls: ['./calibration.component.scss']
})
export class CalibrationComponent implements OnInit {
  private connection: HubConnection;

  public calibrationRunning = false;
  public progress = 0;

  constructor(private message: NzMessageService) {
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
    this.calibrationRunning = true;
    this.message.loading("Calibrating...");

    this.connection.stream(HUB_CALIBRATION_START).subscribe({
      next: (progress: number) => {
        this.progress = progress;
        console.log('[CameraCalibration] Progress: %d', progress);
      },
      complete: () => {
        console.log('[CameraCalibration] Calibration completed');
        this.calibrationRunning = false;
        this.progress = 0;
        this.message.success("Calibration completed!");
      },
      error: (err: any) => {
        console.log('[CameraCalibration] Calibration failed');
        console.log(err);
        this.calibrationRunning = false;
        this.progress = 0;
      }
    });
  }
}
