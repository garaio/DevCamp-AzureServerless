import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PersonsComponent } from './containers/persons.component';

const routes: Routes = [
  { path: '', component: PersonsComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PersonsRoutingModule {}
