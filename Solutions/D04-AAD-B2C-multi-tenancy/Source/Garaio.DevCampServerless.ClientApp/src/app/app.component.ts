import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Subscription } from 'rxjs/Subscription';
import { UserAgentApplication, AuthError, AuthResponse } from 'msal';

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

    this.checkAccount();

    this.authSvcSub.add(this.broadcastService.subscribe('msal:loginSuccess', () => {
      this.checkAccount();
    }));
    this.authSvcSub.add(this.broadcastService.subscribe('msal:loginFailure', (error: any) => {
      this.handleAuthError(error);
    }));
    this.authSvcSub.add(this.broadcastService.subscribe('msal:acquireTokenFailure', (error: any) => {
      this.handleAuthError(error);
    }));
  }

  ngOnDestroy() {
    if (this.authSvcSub) {
      this.authSvcSub.unsubscribe();
    }
  }

  checkAccount() {
    this.loggedIn = !!this.authService.getUser();
  }

  handleAuthError(error: any) {
    if (!this.authService) {
      return;
    }

    const storage = this.authService.getCacheStorage();
    const authError: string = storage.getItem('msal.login.error');

    if ((authError && authError.indexOf('AADB2C90118') > -1) ||
        (error && error.errorDesc && error.errorDesc.indexOf('AADB2C90118') !== -1)) {
      console.log('Reset Password requested');
      this.startResetPasswordFlow();
    // tslint:disable-next-line:max-line-length
    } else if (error && error.error === 'popup_window_error' && error.errorDesc && error.errorDesc.startsWith('Error opening popup window')) {
      this.authService.loginRedirect();
    } else if (this.authService.loginInProgress() === true) {
      console.log('Login is in progress');
    } else if (error && error.error === 'user_login_error' && error.errorDesc && error.errorDesc.startsWith('User login is required')) {
      this.authService.loginRedirect();
    } else if (error && error.error === 'user_cancelled') {
      this.authService.loginRedirect();
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

    resetAuthService.handleRedirectCallback((authErr: AuthError, _response?: AuthResponse) => {
      if (authErr === null) {
        console.log('Reset Password Successful');
      } else {
        console.log(authErr);
      }

      this.authService.loginRedirect();
    });

    resetAuthService.loginRedirect();
  }

  goBack(): void {
    this.location.back();
  }
}
