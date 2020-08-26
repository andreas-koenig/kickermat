import { Component, OnInit, TemplateRef, OnDestroy, ViewChild } from '@angular/core';
import { ApiService } from '@api/api.service';
import { GameState, KickermatPlayer, Game } from '@api/api.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'oth-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit, OnDestroy {
  @ViewChild('players', { static: true }) public playersTmpl!: TemplateRef<any>;
  @ViewChild('game', { static: true }) public gameTmpl!: TemplateRef<any>;
  public template?: TemplateRef<any>;

  public player?: KickermatPlayer;

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    const sub = this.api.getGame().subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private update(game: Game): void {
    switch (game.state) {
      case GameState.NoGame:
        this.template = this.playersTmpl;
        break;
      case GameState.Paused:
      case GameState.Running:
        this.template = this.gameTmpl;
    }

    this.player = game.player;
  }

  public playerChanged(player?: KickermatPlayer): void {
    this.player = player;
  }

  public selectPlayer(player?: KickermatPlayer): void {
    this.player = player;
  }

  public startGame(): void {
    if (!this.player) {
      return;
    }

    const sub = this.api.startGame(this.player).subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }

  public stopGame(): void {
    const sub = this.api.stopGame().subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }
}
