import { RouterModule, Route } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { NotFoundComponent } from './sample/errors/not-found/not-found.component';
import { OverviewComponent } from './overview/containers/overview.component';
import { ProjectsComponent } from './projects/containers/projects.component';
import { PersonsComponent } from './persons/containers/persons.component';
import { TechnologiesComponent } from './technologies/containers/technologies.component';

const routes: Route[] = [
  { path: '', pathMatch: 'full', redirectTo: 'overview' },
  { path: 'overview', component: OverviewComponent },
  { path: 'persons', component: PersonsComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'technologies', component: TechnologiesComponent },

  { path: '**', component: NotFoundComponent }
];

export const AppRoutes: ModuleWithProviders = RouterModule.forRoot(routes);
