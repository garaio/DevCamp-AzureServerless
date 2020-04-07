import { AgmCoreModule } from '@agm/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalModule, MsalInterceptor, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';

import { environment } from 'src/environments/environment';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes.service';

import { SampleModule } from './sample/sample.module';
import { SharedModule } from './shared/shared.module';
import { ShellModule } from './shell/shell.module';
import { OverviewModule } from './overview/overview.module';
import { ProjectsModule } from './projects/projects.module';
import { TechnologiesModule } from './technologies/technologies.module';
import { PersonsModule } from './persons/persons.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AgmCoreModule.forRoot({
      apiKey: ''
    }),
    MsalModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,

    ShellModule,
    SharedModule,
    SampleModule,
    OverviewModule,
    ProjectsModule,
    TechnologiesModule,
    PersonsModule,

    AppRoutes
  ],
  providers: [MsalService,
    {
      provide: MSAL_CONFIG,
      useFactory: () => ({
        auth: {
          authority: environment.authLoginAuthority,
          clientId: environment.authClientId,
          validateAuthority: false,
          redirectUri: environment.authRedirectUri,
          navigateToLoginRequestUrl: true
        }
      })
    },
    {
      provide: MSAL_CONFIG_ANGULAR,
      useFactory: () => ({
        consentScopes: environment.authScopes,
        popUp: false,
        protectedResourceMap: [[environment.apiBaseUrl, environment.authScopes]]
      })
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent],
  schemas: [ NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule { }
