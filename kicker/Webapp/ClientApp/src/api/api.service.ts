import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { KickerParameter, KickerComponent, VideoSource, Channel, MotorDiagnostics } from './api.model';
import { REST_BASE } from './api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public getParameters(component: KickerComponent): Observable<KickerParameter[]> {
    const url = REST_BASE + '/parameters/' + component;

    return this.http.get<KickerParameter[]>(url);
  }

  public setParameter(component: KickerComponent, parameter: string, value: any):
    Observable<any> {
    const url = REST_BASE + '/parameters/' + component + '/' + parameter;

    return this.http.put(url, value);
  }

  public startBallSearch() {
    const url = REST_BASE + '/imageprocessing';
    this.http.get(url).subscribe();
  }

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

  public getMotorDiagnostics(): Observable<MotorDiagnostics[]> {
    const url = REST_BASE + '/motor';

    return this.http.get<MotorDiagnostics[]>(url);
  }
}
