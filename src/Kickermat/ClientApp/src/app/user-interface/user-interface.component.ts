import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { KickermatPlayer, UserInterface } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-interface',
  templateUrl: './user-interface.component.html',
  styleUrls: ['./user-interface.component.scss']
})
export class UserInterfaceComponent implements OnInit, OnDestroy {
  @Input("player") public player!: KickermatPlayer;
  public interfaces: string[] | undefined;
  public uiEnum = UserInterface;

  private subs: Subscription[] = [];

  constructor(private api: ApiService) { }

  ngOnInit() {
    const sub = this.api.getUserInterfaces(this.player)
      .subscribe(resp => this.interfaces = resp);

    this.subs.push(sub);
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }
}
