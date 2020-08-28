import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import {
  KickerParameter, KickermatPlayer, Settings, UpdateSettingsResponse,
  ParameterUpdate, Game, ChannelsResponse, VideoChannel, Camera, UserInterface, Diagnostics, Peripheral
} from './model';
import { REST_BASE } from './api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

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
    settingsName: string,
    paramName: string,
    value: T['value'],
    onSuccess: (newVal: T['value']) => void,
    onError: (oldVal: T['value']) => void
  ): Subscription {
    const url = REST_BASE + '/settings';

    const body: ParameterUpdate = {
      settings: settingsName,
      parameter: paramName,
      value
    };

    return this.http.post<UpdateSettingsResponse<T>>(url, body)
      .subscribe(
        resp => {
          // this.msg.create("success", resp.message as string);
          onSuccess(resp.value);
        },
        error => {
          // this.msg.create("error", error.error.message);
          onError(error.error.value);
        });
  }

  // Motor Diagnostics
  public getMotorDiagnostics(): Observable<Diagnostics> {
    const url = REST_BASE + '/motor';

    return this.http.get<Diagnostics>(url);
  }
}
