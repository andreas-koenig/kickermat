import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import {
  KickerParameter, Motor, KickermatPlayer, Settings, UpdateSettingsResponse,
  ParameterUpdate, Game, ChannelsResponse, VideoChannel, Camera, UserInterface
} from './api.model';
import { REST_BASE } from './api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  // Game
  public getGame(): Observable<Game> {
    const url = REST_BASE + '/game';

    return this.http.get<Game>(url);
  }

  public startGame(player: KickermatPlayer): Observable<Game> {
    const url = REST_BASE + '/game/start';

    const params = new HttpParams()
      .append('player', player.name);

    return this.http.post<Game>(url, undefined, { params });
  }

  public stopGame(): Observable<Game> {
    const url = REST_BASE + '/game/stop';

    return this.http.post<Game>(url, undefined);
  }

  // Players
  public getKickermatPlayers(): Observable<KickermatPlayer[]> {
    const url = REST_BASE + '/player';

    return this.http.get<KickermatPlayer[]>(url);
  }

  public getUserInterfaces(player: KickermatPlayer): Observable<UserInterface[]> {
    const url = REST_BASE + '/ui';

    const params = new HttpParams()
      .append('player', player.name);

    return this.http.get<UserInterface[]>(url, { params });
  }

  // User Interface
  public getVideoChannels(player: KickermatPlayer): Observable<ChannelsResponse> {
    const url = REST_BASE + '/ui/video/channel';

    const params = new HttpParams()
      .append('player', player.name);

    return this.http.get<ChannelsResponse>(url, { params });
  }

  public switchVideoChannel(player: KickermatPlayer, channel: VideoChannel): Observable<HttpResponse<never>> {
    const url = REST_BASE + '/ui/video/channel';

    const params = new HttpParams()
      .append('player', player.name);

    return this.http.post<HttpResponse<never>>(url, channel, { params });
  }

  // Camera
  public getCameras(): Observable<Camera[]> {
    const url = REST_BASE + '/camera';

    return this.http.get<Camera[]>(url);
  }

  public getCamera(name: string): Observable<Camera> {
    const url = REST_BASE + '/camera';

    const params = new HttpParams()
      .append('name', name);

    return this.http.get<Camera>(url, { params });
  }

  // Settings
  public getSettings(id: string): Observable<Settings[]> {
    const url = REST_BASE + '/settings';

    const params = new HttpParams()
      .append('id', id);

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
  public getMotorDiagnostics(): Observable<Motor[]> {
    const url = REST_BASE + '/motor';

    return this.http.get<Motor[]>(url);
  }
}
