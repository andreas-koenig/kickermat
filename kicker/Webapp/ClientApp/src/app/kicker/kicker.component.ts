import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../../api/api.service';
import { Subscription } from 'rxjs';
import { KickermatPlayer } from '../../api/api.model';
import { Router } from '@angular/router';

enum KickermatState {
  Loading,
  Error,
  Ready,
}

@Component({
  selector: 'app-kicker',
  templateUrl: './kicker.component.html',
  styleUrls: ['./kicker.component.scss']
})
export class KickerComponent implements OnInit, OnDestroy {
  public stateEnum = KickermatState;
  public state = KickermatState.Loading;

  public playerSub: Subscription | undefined;
  public players: KickermatPlayer[] = [];

  constructor(private api: ApiService, private router: Router) { }

  ngOnInit() {
    this.playerSub = this.api.getKickermatPlayers().subscribe(
      resp => {
        this.players = [...resp, ...resp, ...resp, ...resp]; // TODO: replace
        this.state = KickermatState.Ready;
      },
      () => this.state = KickermatState.Error
    );
  }

  ngOnDestroy() {
    this.playerSub && this.playerSub.unsubscribe();
  }

  public getAuthors(authors: string[]): string {
    return authors.join(", ");
  }

  public openSettingsModal(player: KickermatPlayer) {
    this.router.navigate(["/settings"], {
      queryParams: { player: player.name }
    });
  }
}
