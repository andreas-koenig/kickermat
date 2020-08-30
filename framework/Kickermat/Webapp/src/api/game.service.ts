import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Subscription, Observable, Subject, from } from 'rxjs';

import { REST_BASE } from './api';
import { Game, KickermatPlayer, GameState } from './model';

@Injectable({
  providedIn: "root",
})
export class GameService implements OnDestroy {
  private gameSubject = new Subject<Game>();
  public game$: Observable<Game> = this.gameSubject.asObservable();

  private subs: Subscription[] = [];

  constructor(private http: HttpClient) {
    const sub = this.getGame().subscribe();
    this.subs.push(sub);
  }
  
  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private intercept<T>(request: Observable<T>, onSuccess: (val: T) => void): Observable<T> {
    const subject = new Subject<T>();

    const sub = request.subscribe(
      response => {
        onSuccess(response);
        subject.next(response);
      },
      error => subject.error(error),
      () => subject.complete());

    this.subs.push(sub);
    return subject.asObservable();
  }

  // Game
  public getGame(): Observable<Game> {
    const url = REST_BASE + '/game';

    return this.intercept(
      this.http.get<Game>(url),
      game => this.gameSubject.next(game));
  }

  public startGame(player: KickermatPlayer): Observable<Game> {
    const url = REST_BASE + '/game/start';

    const params = new HttpParams()
      .append('playerId', player.id);

    return this.intercept(
      this.http.post<Game>(url, undefined, { params }),
      game => this.gameSubject.next(game));
  }

  public stopGame(): Observable<Game> {
    const url = REST_BASE + '/game/stop';

    return this.intercept(
      this.http.post<Game>(url, undefined),
      game => this.gameSubject.next(game));
  }
}
