import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsModule, InputsModule, TableModule, IconsModule, ModalModule } from 'angular-bootstrap-md';

import { OverviewComponent } from './containers/overview.component';
import { OverviewRoutingModule } from './overview-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ModalModule,
    FormsModule,
    ButtonsModule,
    InputsModule,
    IconsModule,
    TableModule,

    OverviewRoutingModule
  ],
  declarations: [
    OverviewComponent
  ],
  exports: [OverviewComponent],
  entryComponents: []
})
export class OverviewModule { }
