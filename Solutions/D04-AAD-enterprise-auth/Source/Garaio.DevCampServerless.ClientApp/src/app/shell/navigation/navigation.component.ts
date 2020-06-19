import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { UserAgentApplication, AuthError, AuthResponse } from 'msal';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit, OnDestroy {
  @ViewChild('sidenav', {static: true}) sidenav: ElementRef;

  clicked: boolean;

  userName$: Observable<string>;
  authSvcSub: Subscription = new Subscription();

  constructor(
    private http: HttpClient,
    private broadcastService: BroadcastService,
    private authService: MsalService) {
    this.clicked = this.clicked === undefined ? false : true;
  }

  ngOnInit() {
    this.initUserProfile();

    this.authSvcSub.add(this.broadcastService.subscribe('msal:loginSuccess', () => {
      this.initUserProfile();
    }));
  }

  ngOnDestroy() {
    if (this.authSvcSub) {
      this.authSvcSub.unsubscribe();
    }
  }

  initUserProfile() {
    const url = `${environment.apiBaseUrl}/user?code=${environment.apiAuthCode}`;
    this.userName$ = this.http.get(url, {responseType: 'text'});
  }

  setClicked(val: boolean): void {
    this.clicked = val;
  }

  onShowProfile(): void {
    const profileEditService = new UserAgentApplication({
      auth: {
        clientId: environment.authClientId,
        authority: environment.authChangeAuthority,
        validateAuthority: false,
        redirectUri: environment.authRedirectUri,
        navigateToLoginRequestUrl: true
      }
    });

    profileEditService.handleRedirectCallback((authErr: AuthError, _response?: AuthResponse) => {
      if (authErr === null) {
        console.log('Profile Editing Successful');
      } else {
        console.log(authErr);
      }
    });

    profileEditService.loginRedirect();
  }

  onLogout(): void {
    this.authService.logout();
  }
}
