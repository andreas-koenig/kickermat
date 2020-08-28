import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

import { ApiService } from '@api/api.service';
import { KickermatPlayer } from '@api/model';

type Status = 'loading' | 'error' | 'done';

@Component({
  selector: 'oth-players',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.scss']
})
export class PlayersListComponent implements OnInit, OnDestroy {
  private subs: Subscription[] = [];

  @Output('selected')
  public playerChanged = new EventEmitter<KickermatPlayer | undefined>();

  public players: KickermatPlayer[] | undefined;
  public selectedIndex: number | undefined;
  public status: Status = 'loading';

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    const sub = this.api.getKickermatPlayers()
      .subscribe(
        players => {
          this.players = players;
          this.status = 'done';
        },
        error => console.log(`Could not query players: ${error}`)
      );

    this.subs.push(sub);

    window.addEventListener('click', this.deselect.bind(this));
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
    window.removeEventListener('click', this.deselect.bind(this));
  }

  public selectPlayer(event: MouseEvent, index: number): void {
    event.stopPropagation();
    event.preventDefault();
    this.selectedIndex = index;
    this.playerChanged.emit(this.players[index]);
  }

  public deselect(): void {
    this.selectedIndex = undefined;
    this.playerChanged.emit(undefined);
  }
}
