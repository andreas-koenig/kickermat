import { Component } from '@angular/core';

@Component({
  selector: 'oth-root',
  templateUrl: './app.component.html',
  styles: [
    `main { min-height: calc(100vh - 48px)}`,
  ]
})
export class AppComponent {
  public organization = 'OTH';
  public application = 'Kickermat';
}
