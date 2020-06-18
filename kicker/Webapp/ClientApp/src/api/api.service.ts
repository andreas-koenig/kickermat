import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';

import {
  KickerParameter, KickerComponent, VideoSource,
  Channel, Motor, KickermatPlayer, Settings, UpdateSettingsResponse,
  ParameterUpdate, Game
} from './api.model';
import { REST_BASE } from './api';
import { NzMessageService } from 'ng-zorro-antd';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient, private msg: NzMessageService) { }

  // Game
  public getGame(): Observable<Game> {
    const url = REST_BASE + '/game';

    return this.http.get<Game>(url);
  }

  public startGame(player: KickermatPlayer): Observable<Game> {
    const url = REST_BASE + '/game/start';

    const params = new HttpParams()
      .append("player", player.name);

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

  // Settings
  public getPlayerSettings(playerName: string): Observable<Settings[]> {
    const url = REST_BASE + '/settings';

    const params = new HttpParams()
      .append("player", playerName);

    return this.http.get<Settings[]>(url, { params });
  }

  public updateParam<T extends KickerParameter<any>>(
    settingsName: string,
    paramName: string,
    value: T["value"],
    onSuccess: (newVal: T["value"]) => void,
    onError: (oldVal: T["value"]) => void
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
          this.msg.create("success", resp.message as string);
          onSuccess(resp.value);
        },
        error => {
          this.msg.create("error", error.error.message);
          onError(error.error.value);
        });
  }

  // Parameters
  public getParameters(component: KickerComponent): Observable<KickerParameter<any>[]> {
    const url = REST_BASE + '/parameters/' + component;

    return this.http.get<KickerParameter<any>[]>(url);
  }

  public setParameter(component: KickerComponent, parameter: string, value: any):
    Observable<any> {
    const url = REST_BASE + '/parameters/' + component + '/' + parameter;

    return this.http.put(url, value);
  }

  // ImageProcessing
  public startBallSearch() {
    const url = REST_BASE + '/imageprocessing';
    this.http.get(url).subscribe();
  }

  // VideoSource
  public getChannels(videoSource: VideoSource): Observable<Channel[]> {
    const url = REST_BASE + '/video/' + videoSource + '/channels';

    return this.http.get<Channel[]>(url);
  }

  public getChannel(videoSource: VideoSource): Observable<Channel> {
    const url = REST_BASE + '/video/' + videoSource + '/channel';

    return this.http.get<Channel>(url);
  }

  public switchChannel(videoSource: VideoSource, channel: Channel): Observable<Channel> {
    const url = REST_BASE + '/video/' + videoSource + '/channel';

    return this.http.put<Channel>(url, channel);
  }

  // Motor Diagnostics
  public getMotorDiagnostics(): Observable<Motor[]> {
    const url = REST_BASE + '/motor';

    return this.http.get<Motor[]>(url);
  }
}
