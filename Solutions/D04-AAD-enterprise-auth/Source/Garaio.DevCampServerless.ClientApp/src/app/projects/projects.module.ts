import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonsModule, InputsModule, TableModule, IconsModule, ModalModule } from 'angular-bootstrap-md';

import { ProjectsComponent } from './containers/projects.component';
import { ProjectsRoutingModule } from './projects-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ProjectsModalComponent } from './components/modal/projects-modal.component';
import { ProjectsListComponent } from './components/list/projects-list.component';

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

    ProjectsRoutingModule
  ],
  declarations: [
    ProjectsComponent,
    ProjectsModalComponent,
    ProjectsListComponent
  ],
  exports: [ProjectsComponent],
  entryComponents: [
    ProjectsModalComponent
  ]
})
export class ProjectsModule { }
