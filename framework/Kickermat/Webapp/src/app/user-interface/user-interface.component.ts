import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { KickermatPlayer, UserInterface } from '@api/model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'oth-user-interface',
  templateUrl: './user-interface.component.html',
})
export class UserInterfaceComponent implements OnInit, OnDestroy {
  @Input('player') public player!: KickermatPlayer;
  public interfaces: UserInterface[] | undefined;
  public uiEnum = UserInterface;

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    const sub = this.api.getUserInterfaces(this.player)
      .subscribe(resp => this.interfaces = resp);

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }
}
