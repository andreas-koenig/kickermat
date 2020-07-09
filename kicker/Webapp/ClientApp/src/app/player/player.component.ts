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

  private deselect: () => void;

  constructor(private api: ApiService) {
    this.deselect = (() => {
      this.selectPlayer(undefined);
    }).bind(this);
  }

  ngOnInit() {
    this.playerSub = this.api.getKickermatPlayers().subscribe(
      resp => {
        this.players = resp;
        this.state = State.Ready;
      },
      () => this.state = State.Error
    );

    window.addEventListener("click", this.deselect);
  }

  ngOnDestroy() {
    this.playerSub && this.playerSub.unsubscribe();
    window.removeEventListener("click", this.deselect);
  }

  selectPlayer(player?: KickermatPlayer, event?: MouseEvent) {
    event && event.stopPropagation();
    this.selectedPlayer = player;
    this.selectedEvent.emit(player);
  }
}
