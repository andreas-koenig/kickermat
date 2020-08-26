import { Component, Input } from '@angular/core';

@Component({
  selector: 'oth-page-heading',
  template: `
    <ng-container *ngIf="!skeleton">
      <h2>{{subHeading}}</h2>
      <h1>{{heading}}</h1>
    </ng-container>

    <ng-container *ngIf="skeleton">
      <ibm-skeleton-text [lines]="2"></ibm-skeleton-text>
    </ng-container>
  `,
  styleUrls: ['./page-heading.component.scss']
})
export class PageHeadingComponent {
  @Input('heading') public heading: string;
  @Input('subHeading') public subHeading: string;
  @Input('skeleton') public skeleton: false;
}
