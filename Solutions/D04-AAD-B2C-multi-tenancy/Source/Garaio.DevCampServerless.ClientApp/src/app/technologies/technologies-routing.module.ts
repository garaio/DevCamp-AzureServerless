import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TechnologiesComponent } from './containers/technologies.component';

const routes: Routes = [
  { path: '', component: TechnologiesComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class TechnologiesRoutingModule {}
