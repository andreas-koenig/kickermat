import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Motor } from '@api/api.model';

export type Selection = "bar" | "nmtState" | "canOpenId"
  | "operatingState" | "operatingMode" | "error";

@Component({
  selector: 'app-info-table',
  templateUrl: './info-table.component.html',
  styleUrls: ['./info-table.component.scss']
})
export class InfoTableComponent implements OnInit {
  @Input('motor') public motor: Motor | undefined;
  @Output('onChange') public itemChanged: EventEmitter<Selection> = new EventEmitter();

  public selection: Selection = "bar";

  constructor() { }

  ngOnInit() {
    this.selectItem("bar");
  }

  public selectItem(selection: Selection) {
    this.selection = selection;
    this.itemChanged.emit(this.selection);
  }
}
