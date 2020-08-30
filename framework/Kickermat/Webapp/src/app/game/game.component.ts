import { Component, OnInit, TemplateRef, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { GameService } from '@api/game.service';
import { GameState, KickermatPlayer, Game } from '@api/model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'oth-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit, OnDestroy {
  private subs: Subscription[] = [];
  game: Game | undefined;
  player: KickermatPlayer | undefined;

  constructor(private gameService: GameService) { }

  ngOnInit() {
    const sub = this.gameService.game$.subscribe(game => {
      this.game = game;
      if (!this.player && this.isGameRunning()) {
        this.player = this.game.player;
      }
    });
    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
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

    const sub = this.gameService.startGame(this.player).subscribe();
    this.subs.push(sub);
  }

  isGameRunning(): boolean {
    return this.game ? this.game.state === GameState.Running : false;
  }
}
