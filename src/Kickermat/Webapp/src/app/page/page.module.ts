import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// Carbon Module
import { CarbonModule } from '@app/carbon.module';

// Kickermat Components
import { PageHeadingComponent } from './page-heading.component';

@NgModule({
  declarations: [
    PageHeadingComponent
  ],
  imports: [
    BrowserModule,
    CarbonModule,
  ],
  exports: [
    PageHeadingComponent
  ]
})
export class PageModule { }
