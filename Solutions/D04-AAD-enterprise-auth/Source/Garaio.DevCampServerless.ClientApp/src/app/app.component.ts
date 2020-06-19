import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Subscription } from 'rxjs/Subscription';
import { UserAgentApplication, AuthError, AuthResponse, LogLevel, Logger } from 'msal';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.css']

})

export class AppComponent implements OnInit, OnDestroy {
  values: string[] = ['Tag 1', 'Tag 2', 'Tag 4'];

  specialPage: boolean;
  isIframe = false;
  loggedIn = false;

  authSvcSub: Subscription = new Subscription();

  private specialPages: any[] = [
    '/pages/login',
    '/pages/register',
    '/pages/lock',
    '/pages/pricing',
    '/pages/single-post',
    '/pages/post-listing'
  ];

  private currentUrl = '';

  constructor(
    private router: Router,
    private location: Location,
    private broadcastService: BroadcastService,
    private authService: MsalService
  ) {

    this.router.events.subscribe((route: any) => {
      this.currentUrl = route.url;

      this.specialPage = this.specialPages.indexOf(this.currentUrl) !== -1;
    });
  }

  ngOnInit(): void {
    this.isIframe = window !== window.parent && !window.opener;

    this.authService.handleRedirectCallback((authError, response) => {
      if (authError) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }

      console.log('Redirect Success: ', response);
    });

    this.authService.setLogger(new Logger(this.loggerCallback));

    this.authSvcSub.add(this.broadcastService.subscribe('msal:loginSuccess', () => {
      this.checkAccount();
    }));
    this.authSvcSub.add(this.broadcastService.subscribe('msal:loginFailure', (error: any) => {
      this.handleAuthError(error);
    }));
    this.authSvcSub.add(this.broadcastService.subscribe('msal:acquireTokenFailure', (error: any) => {
      this.handleAuthError(error);
    }));

    this.checkAccount();
  }

  ngOnDestroy() {
    if (this.authSvcSub) {
      this.authSvcSub.unsubscribe();
    }
  }

  checkAccount() {
    const account = this.authService.getAccount();

    console.log(account || 'No authentication data available');

    this.loggedIn = !!account;
  }

  loggerCallback(level: LogLevel, message: string, containsPii: boolean) {
    if (environment.production) {
      if (level > LogLevel.Warning || containsPii) { // Note: PII means 'Personal Identity Information'
        return;
      }
    }

    console.log(message);
  }

  handleAuthError(error: any) {
    if (!this.authService) {
      return;
    }

    // See https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-handling-exceptions?tabs=javascript
    let authError = error as AuthError;
    if (!!!authError && error && error.error) {
      authError = new AuthError(error.error, error.errorDesc);
    }
    console.log(authError);

    if (authError.errorMessage && authError.errorMessage.indexOf('AADB2C90118') !== -1) {
      console.log('Reset Password requested');
      this.startResetPasswordFlow();
    // tslint:disable-next-line:max-line-length
    } else if (authError.errorCode === 'popup_window_error' && authError.errorMessage && authError.errorMessage.startsWith('Error opening popup window')) {
      this.authService.loginRedirect({scopes: [...environment.authGeneralScopes, ...environment.authApiScopes]});
    } else if (this.authService.getLoginInProgress() === true) {
      console.log('Login is in progress');
    // tslint:disable-next-line:max-line-length
    } else if (authError.errorCode === 'user_login_error' && authError.errorMessage && authError.errorMessage.startsWith('User login is required')) {
      this.authService.loginRedirect({scopes: [...environment.authGeneralScopes, ...environment.authApiScopes]});
    } else if (authError.errorCode === 'user_cancelled') {
      this.authService.loginRedirect({scopes: [...environment.authGeneralScopes, ...environment.authApiScopes]});
    } else {
      console.log(error);
      this.router.navigate(['error']);
    }
  }

  startResetPasswordFlow() {
    const resetAuthService = new UserAgentApplication({
      auth: {
        clientId: environment.authClientId,
        authority: environment.authResetAuthority,
        validateAuthority: false,
        redirectUri: environment.authRedirectUri,
        navigateToLoginRequestUrl: true
      }
    });

    resetAuthService.setLogger(new Logger(this.loggerCallback));

    resetAuthService.handleRedirectCallback((authErr: AuthError, _response?: AuthResponse) => {
      if (authErr === null) {
        console.log('Reset Password Successful');
      } else {
        console.log(authErr);
      }

      this.authService.loginRedirect({scopes: [...environment.authGeneralScopes, ...environment.authApiScopes]});
    });

    resetAuthService.loginRedirect({scopes: [...environment.authGeneralScopes, ...environment.authApiScopes]});
  }

  goBack(): void {
    this.location.back();
  }
}
