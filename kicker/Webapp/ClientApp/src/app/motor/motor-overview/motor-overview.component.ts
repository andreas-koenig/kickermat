import { Component, ElementRef, ViewChild, EventEmitter, Output, OnInit } from '@angular/core';
import { Motor } from '@api/api.model';
import { Observable } from 'rxjs';
import { ApiService } from '@api/api.service';

@Component({
  selector: 'app-motor-overview',
  templateUrl: './motor-overview.component.html',
  styleUrls: ['./motor-overview.component.scss']
})
export class MotorOverviewComponent implements OnInit {
  @ViewChild('cards', { static: true }) public cards!: ElementRef<HTMLElement>;
  private currentCard = 0;

  @Output('onChange') motorChanged: EventEmitter<Motor> = new EventEmitter();
  public motors$: Observable<Motor[]> | undefined;
  public selectedMotor!: Motor;
  public numMotors = 0;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.motors$ = this.api.getMotorDiagnostics();
    this.motors$.subscribe(motors => {
      this.numMotors = motors.length;
      motors.length > 0 && this.selectMotor(motors[0], 0)
    });
  }

  public mouseWheel(event: WheelEvent) {
    event.preventDefault();
    event.stopPropagation();
    event.deltaY > 0 ? this.scrollRight() : this.scrollLeft();
  }

  public scrollLeft() {
    if (this.currentCard > 0) {
      this.currentCard--;
      this.scroll('end');
    }
  }

  public scrollRight() {
    if (this.currentCard < this.numMotors - 1) {
      this.currentCard++;
      this.scroll('start');
    }
  }

  public scroll(inline: 'start' | 'center' | 'end' | 'nearest' | undefined) {
    const card = this.cards.nativeElement.children.item(this.currentCard);

    card && card.scrollIntoView({
      behavior: 'smooth',
      inline: inline,
      block: 'nearest'
    });
  }

  public selectMotor(motor: Motor, index: number) {
    this.selectedMotor = motor;
    this.motorChanged.emit(motor);
    this.currentCard = index;
    this.scroll('center');
  }
}
