import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsModule, InputsModule, TableModule, IconsModule, ModalModule } from 'angular-bootstrap-md';

import { SharedModule } from '../shared/shared.module';
import { PersonsComponent } from './containers/persons.component';
import { PersonsRoutingModule } from './persons-routing.module';
import { PersonsModalComponent } from './components/modal/persons-modal.component';
import { PersonsListComponent } from './components/list/persons-list.component';

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

    PersonsRoutingModule
  ],
  declarations: [
    PersonsComponent,
    PersonsModalComponent,
    PersonsListComponent
  ],
  exports: [PersonsComponent],
  entryComponents: [
    PersonsModalComponent
  ]
})
export class PersonsModule { }
