import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ApiService } from '../../api/api.service';
import { Subscription } from 'rxjs';
import { KickermatPlayer } from '../../api/api.model';

enum State {
  Loading,
  Error,
  Ready,
}

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit, OnDestroy {
  @Output("selected") public selectedEvent = new EventEmitter<KickermatPlayer>();
  public selectedPlayer?: KickermatPlayer;

  public stateEnum = State;
  public state = State.Loading;

  public playerSub: Subscription | undefined;
  public players: KickermatPlayer[] = [];

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.playerSub = this.api.getKickermatPlayers().subscribe(
      resp => {
        this.players = [...resp, ...resp, ...resp, ...resp]
          .map((player, i) => (
            i === 0
              ? player
              : {
                  ...player,
                  ...{ name: `${player.name} ${i}` }
              })
          ); // TODO: replace
        this.state = State.Ready;
      },
      () => this.state = State.Error
    );
  }

  ngOnDestroy() {
    this.playerSub && this.playerSub.unsubscribe();
  }

  selectPlayer(player: KickermatPlayer, event?: MouseEvent) {
    event && event.stopPropagation();
    this.selectedPlayer = player;
    this.selectedEvent.emit(player);
  }
}
