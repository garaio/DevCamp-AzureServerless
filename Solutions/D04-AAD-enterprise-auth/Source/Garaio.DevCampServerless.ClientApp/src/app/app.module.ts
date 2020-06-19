import { AgmCoreModule } from '@agm/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA, ApplicationRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalModule, MsalInterceptor, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';

import { environment } from 'src/environments/environment';
import { AppComponent } from './app.component';
import { MsalComponent } from './msal.component';
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
    AppComponent,
    MsalComponent
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
          postLogoutRedirectUri: environment.authRedirectUri
        }
      })
    },
    {
      provide: MSAL_CONFIG_ANGULAR,
      useFactory: () => ({
        consentScopes: [...environment.authGeneralScopes, ...environment.authApiScopes],
        popUp: false,
        protectedResourceMap: [[environment.apiBaseUrl, environment.authApiScopes]]
      })
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    }
  ],
  entryComponents: [
    AppComponent,
    MsalComponent
  ],
  schemas: [ NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule {
  // Source: https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-js-avoid-page-reloads#initialization-in-your-main-app-file
  ngDoBootstrap(ref: ApplicationRef) {
    if (window !== window.parent && !window.opener) {
      console.log('Bootstrap: MSAL');
      ref.bootstrap(MsalComponent);
    } else {
      // this.router.resetConfig(RouterModule);
      console.log('Bootstrap: App');
      ref.bootstrap(AppComponent);
    }
  }
}
