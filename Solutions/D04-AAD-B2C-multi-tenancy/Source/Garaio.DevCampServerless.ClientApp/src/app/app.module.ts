import { AgmCoreModule } from '@agm/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalModule, MsalInterceptor } from '@azure/msal-angular';
import { LogLevel } from 'msal';

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

// Logger callback for MSAL
export function loggerCallback(level: LogLevel, message: string, containsPii: boolean) {
  if (environment.production) {
    if (level > LogLevel.Warning || containsPii) { // Note: PII means 'Personal Identity Information'
      return;
    }
  }

  console.log(message);
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AgmCoreModule.forRoot({
      apiKey: ''
    }),
    // Note: The properties of this configuration are a bit re-structured in next - not yet official - version (they can be easily remapped)
    MsalModule.forRoot({
      authority: environment.authLoginAuthority,
      clientID: environment.authClientId,
      validateAuthority: false,
      consentScopes: environment.authScopes,
      protectedResourceMap: [[environment.apiBaseUrl, environment.authScopes]],
      redirectUri: environment.authRedirectUri,
      popUp: false,
      logger: loggerCallback,
      level: LogLevel.Verbose,
      navigateToLoginRequestUrl: true
    }),
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
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: MsalInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent],
  schemas: [ NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule { }
