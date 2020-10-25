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
  NotificationModule,
} from 'carbon-components-angular';

// Carbon Icons
import {
  RenewModule,
  Network_2Module,
  InformationFilledModule,
  OperationsRecordModule,
  ArrowsVerticalModule,
  RotateModule,
  StopFilledModule,
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
    NotificationModule,

    // Icons
    RenewModule,
    Network_2Module,
    InformationFilledModule,
    OperationsRecordModule,
    ArrowsVerticalModule,
    RotateModule,
    StopFilledModule,
  ],
})
export class CarbonModule { }
