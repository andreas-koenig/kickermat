import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// Carbon Module
import { CarbonModule } from '@app/carbon.module';

// Components
import { SettingsComponent } from './settings.component';
import { BooleanParameterComponent } from './boolean-param/boolean-param.component';
import { ColorRangeParamComponent } from './color-range-param/color-range-param.component';
import { EnumParamComponent } from './enum-param/enum-param.component';
import { NumberParamComponent } from './number-param/number-param.component';

@NgModule({
  declarations: [
    SettingsComponent,
    BooleanParameterComponent,
    ColorRangeParamComponent,
    EnumParamComponent,
    NumberParamComponent,
  ],
  exports: [SettingsComponent],
  imports: [
    BrowserModule,
    CarbonModule,
  ],
})
export class SettingsModule { }
