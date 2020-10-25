import { Component, OnInit, HostBinding, Input, OnDestroy } from '@angular/core';
import { NavigationExtras } from '@angular/router';

import { Subscription } from 'rxjs';

import { ApiService } from '@api/api.service';
import { GameService } from '@api/game.service';
import { Camera, Game, GameState } from '@api/model';

@Component({
  selector: 'oth-navigation',
  templateUrl: './navigation.component.html',
  styles: [
    '.stop-button: { margin-right: .5rem; }'
  ]
})
export class NavigationComponent implements OnInit, OnDestroy {
  @Input('organization') public organization: string;
  @Input('application') public application: string;

  @HostBinding('class.bx--header') headerClass = true;

  public cameras: Camera[] = [];
  game: Game | undefined;
  private subs: Subscription[] = [];

  constructor(private api: ApiService, private gameService: GameService) { }

  ngOnInit(): void {
    const sub1 = this.api.getCameras()
      .subscribe(
        cameras => this.cameras = cameras,
        error => console.log(`Failed to query cameras: ${error}`)
      );

    const sub2 = this.gameService.game$.subscribe(game => this.game = game);

    this.subs.push(sub1, sub2);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  public getRouteExtras(camera: Camera): NavigationExtras {
    return { queryParams: { id: camera.id } };
  }

  public stopGame() {
    const sub = this.gameService.stopGame().subscribe();
    this.subs.push(sub);
  }

  isGameRunning(): boolean {
    return this.game ? this.game.state === GameState.Running : false;
  }
}
