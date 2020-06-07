import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  KickerParameter, KickerComponent, VideoSource,
  Channel, Motor, KickermatPlayer
} from './api.model';
import { REST_BASE } from './api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  // Game & Players
  public getKickermatPlayers(): Observable<KickermatPlayer[]> {
    const url = REST_BASE + '/kickermat/players';

    return this.http.get<KickermatPlayer[]>(url);
  }

  // Parameters
  public getParameters(component: KickerComponent): Observable<KickerParameter[]> {
    const url = REST_BASE + '/parameters/' + component;

    return this.http.get<KickerParameter[]>(url);
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
