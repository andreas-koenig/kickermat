import { Component, Input } from '@angular/core';

@Component({
  selector: 'oth-heading',
  template: `
    <ng-container *ngIf="!skeleton && heading">
      <h2>{{subHeading}}</h2>
      <div class="heading">
        <h1>{{heading}}</h1>
        <ng-content select="oth-peripheral-state">
        </ng-content>
      </div>
    </ng-container>

    <ng-container *ngIf="skeleton">
      <ibm-skeleton-text [lines]="2"></ibm-skeleton-text>
    </ng-container>
  `,
  styleUrls: [ './heading.component.scss' ],
})
export class HeadingComponent {
  @Input() heading: string | undefined;
  @Input() subHeading: string | undefined;
  @Input() skeleton = false;
}
