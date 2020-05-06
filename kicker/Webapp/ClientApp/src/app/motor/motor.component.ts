import { Component, OnInit, ViewChild, ElementRef, ViewChildren, QueryList } from '@angular/core';
import { ApiService } from '@api/api.service';
import { MotorDiagnostics } from '@api/api.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-motor',
  templateUrl: './motor.component.html',
  styleUrls: ['./motor.component.scss']
})
export class MotorComponent implements OnInit {
  public diagnostics: Observable<MotorDiagnostics[]> | undefined;

  private currentCard = 0;

  @ViewChild('cards', { static: true })
  public cards!: ElementRef<HTMLElement>;

  @ViewChildren('motorcard')
  public motorCards!: QueryList<ElementRef<HTMLElement>>;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.diagnostics = this.api.getMotorDiagnostics();
  }

  public mouseScroll(event: WheelEvent) {
    event.preventDefault();
    event.deltaY > 0 ? this.scrollRight() : this.scrollLeft();
  }

  public scrollLeft() {
    if (this.currentCard > 0) {
      this.currentCard--;
      this.scroll('end');
    }
  }

  public scrollRight() {
    if (this.currentCard < this.motorCards.length - 1) {
      this.currentCard++;
      this.scroll('start');
    }
  }

  public scroll(inline: 'start' | 'center' | 'end' | 'nearest' | undefined) {
    const card = this.cards.nativeElement.children.item(this.currentCard);

    card && card.scrollIntoView({
      behavior: 'smooth',
      inline: inline,
    });
  }
}
