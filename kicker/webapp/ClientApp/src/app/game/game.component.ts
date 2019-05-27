import { Component, OnInit } from '@angular/core';
import { NavItem } from '@app/navigation/navigation';

@NavItem({
  name: 'Game',
  icon: 'game',
  path: 'game',
  component: GameComponent
})
@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
