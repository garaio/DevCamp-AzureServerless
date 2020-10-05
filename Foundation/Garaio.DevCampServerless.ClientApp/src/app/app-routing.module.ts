import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OverviewComponent } from './overview/containers/overview.component';
import { ProjectsComponent } from './projects/containers/projects.component';
import { PersonsComponent } from './persons/containers/persons.component';
import { TechnologiesComponent } from './technologies/containers/technologies.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'overview' },
  { path: 'overview', component: OverviewComponent },
  { path: 'persons', component: PersonsComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'technologies', component: TechnologiesComponent },

  { path: '**', redirectTo: 'overview' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
