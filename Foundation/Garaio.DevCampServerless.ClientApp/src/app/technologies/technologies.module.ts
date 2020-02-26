import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsModule, InputsModule, TableModule, IconsModule, ModalModule } from 'angular-bootstrap-md';

import { SharedModule } from '../shared/shared.module';
import { TechnologiesComponent } from './containers/technologies.component';
import { TechnologiesRoutingModule } from './technologies-routing.module';
import { TechnologiesModalComponent } from './components/modal/technologies-modal.component';
import { TechnologiesListComponent } from './components/list/technologies-list.component';

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

    TechnologiesRoutingModule
  ],
  declarations: [
    TechnologiesComponent,
    TechnologiesModalComponent,
    TechnologiesListComponent
  ],
  exports: [TechnologiesComponent],
  entryComponents: [
    TechnologiesModalComponent
  ]
})
export class TechnologiesModule { }
