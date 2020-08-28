import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// Carbon Module
import { CarbonModule } from '@app/carbon.module';

// Kickermat Components
import { PageComponent } from './page.component';
import { HeadingComponent } from './heading/heading.component';
import { PeripheralStateComponent } from './peripheral-state/peripheral-state.component';

@NgModule({
  declarations: [
    PageComponent,
    HeadingComponent,
    PeripheralStateComponent,
  ],
  imports: [
    BrowserModule,
    CarbonModule,
  ],
  exports: [
    PageComponent,
    HeadingComponent,
    PeripheralStateComponent,
  ]
})
export class PageModule { }
