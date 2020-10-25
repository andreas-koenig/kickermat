import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import {
  KickerParameter, KickermatPlayer, Settings, UpdateSettingsResponse,
  ParameterUpdate, Game, ChannelsResponse, VideoChannel, Camera, UserInterface, Diagnostics, Peripheral
} from './model';
import { REST_BASE } from './api';
import { NotificationService, NotificationContent } from 'carbon-components-angular';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient, private notificationService: NotificationService) { }

  // Players
  public getKickermatPlayers(): Observable<KickermatPlayer[]> {
    const url = REST_BASE + '/player';

    return this.http.get<KickermatPlayer[]>(url);
  }

  public getUserInterfaces(player: KickermatPlayer): Observable<UserInterface[]> {
    const url = REST_BASE + '/ui';

    const params = new HttpParams()
      .append('playerId', player.id);

    return this.http.get<UserInterface[]>(url, { params });
  }

  // User Interface
  public getVideoChannels(player: KickermatPlayer): Observable<ChannelsResponse> {
    const url = REST_BASE + '/ui/video/channel';

    const params = new HttpParams()
      .append('playerId', player.id);

    return this.http.get<ChannelsResponse>(url, { params });
  }

  public switchVideoChannel(player: KickermatPlayer, channel: VideoChannel): Observable<HttpResponse<never>> {
    const url = REST_BASE + '/ui/video/channel';

    const params = new HttpParams()
      .append('playerId', player.id);

    return this.http.post<HttpResponse<never>>(url, channel, { params });
  }

  // Camera
  public getCameras(): Observable<Camera[]> {
    const url = REST_BASE + '/camera';

    return this.http.get<Camera[]>(url);
  }

  public getCamera(cameraId: string): Observable<Camera> {
    const url = REST_BASE + '/camera';

    const params = new HttpParams()
      .append('id', cameraId);

    return this.http.get<Camera>(url, { params });
  }

  // Settings
  public getSettings(entityId: string): Observable<Settings[]> {
    const url = REST_BASE + '/settings';

    const params = new HttpParams()
      .append('entityId', entityId);

    return this.http.get<Settings[]>(url, { params });
  }

  public updateParam<T extends KickerParameter<any>>(
    settingsId: string,
    paramName: string,
    newValue: T['value'],
    currentValue: T['value'],
    onSuccess: (newVal: T['value']) => void,
    onError: (oldVal: T['value']) => void
  ): Subscription {
    const url = REST_BASE + '/settings';

    const body: ParameterUpdate = {
      settingsId: settingsId,
      parameter: paramName,
      value: newValue,
    };

    return this.http.post<UpdateSettingsResponse<T>>(url, body)
      .subscribe(
        resp => {
          onSuccess(resp.value);
          
          this.showNotification("success", "Success", `Successfully updated ${paramName}`);
        },
        error => {
          error.error.value
            ? onError(error.error.value)
            : onError(currentValue);

          this.showNotification("error", "Failure", error.error.message);
        });
  }

  // Motor Diagnostics
  public getMotorDiagnostics(): Observable<Diagnostics> {
    const url = REST_BASE + '/motor';

    return this.http.get<Diagnostics>(url);
  }

  private showNotification(type: "error" | "success", title: string, message: string) {
    this.notificationService.showNotification({
      title,
      type,
      message,
      smart: true,
      target: "#notification-outlet",
    });
  }
}
