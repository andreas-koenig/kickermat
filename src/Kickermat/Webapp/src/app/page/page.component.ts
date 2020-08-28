import { Component } from '@angular/core';

@Component({
  selector: 'oth-page',
  template: `
    <div ibmGrid>
      <!-- Heading -->
      <div ibmRow id="heading">
        <div ibmCol>
          <ng-content select="oth-heading"></ng-content>
        </div>
      </div>

      <!-- Page content -->
      <ng-content></ng-content>
    </div>
  `,
  styleUrls: [ 'page.component.scss' ],
})
export class PageComponent {}
