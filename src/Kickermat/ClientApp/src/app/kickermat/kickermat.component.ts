import { Component, OnInit, TemplateRef, OnDestroy, ViewChild } from '@angular/core';
import { ApiService } from '@api/api.service';
import { GameState, KickermatPlayer, Game } from '@api/api.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-kickermat',
  templateUrl: './kickermat.component.html',
  styleUrls: ['./kickermat.component.scss']
})
export class KickermatComponent implements OnInit, OnDestroy {
  @ViewChild("players", { static: true }) public playersTmpl!: TemplateRef<any>;
  @ViewChild("game", { static: true }) public gameTmpl!: TemplateRef<any>;
  public template?: TemplateRef<any>;

  public player?: KickermatPlayer;

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit() {
    const sub = this.api.getGame().subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  private update(game: Game) {
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

  public selectPlayer(player?: KickermatPlayer) {
    this.player = player;
  }

  public startGame() {
    if (!this.player) {
      return;
    }

    const sub = this.api.startGame(this.player).subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }

  public stopGame() {
    const sub = this.api.stopGame().subscribe(
      resp => this.update(resp)
    );

    this.subs.push(sub);
  }
}
