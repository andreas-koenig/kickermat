import { NgModule } from '@angular/core';

// Carbon Modules
import {
  UIShellModule,
  ButtonModule,
  GridModule,
  TilesModule,
  SkeletonModule,
  LoadingModule,
  ContentSwitcherModule,
  TabsModule,
  TagModule,
  ToggleModule,
  SliderModule,
  SelectModule,
} from 'carbon-components-angular';

// Carbon Icons
import {
  RenewModule,
  Network_2Module,
  InformationFilledModule,
  OperationsRecordModule,
  ArrowsVerticalModule,
  RotateModule,
} from '@carbon/icons-angular';

@NgModule({
  exports: [
    // Component Modules
    UIShellModule,
    ButtonModule,
    GridModule,
    TilesModule,
    SkeletonModule,
    LoadingModule,
    ContentSwitcherModule,
    TabsModule,
    TagModule,
    ToggleModule,
    SliderModule,
    SelectModule,

    // Icons
    RenewModule,
    Network_2Module,
    InformationFilledModule,
    OperationsRecordModule,
    ArrowsVerticalModule,
    RotateModule,
  ],
})
export class CarbonModule { }
