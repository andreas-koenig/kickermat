import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { KickerParameter, KickerComponent } from './api.model';
import { API_BASE } from './api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public getParameters(component: KickerComponent): Observable<KickerParameter[]> {
    const url = API_BASE + '/parameters/' + component;

    return this.http.get<KickerParameter[]>(url);
  }
}
