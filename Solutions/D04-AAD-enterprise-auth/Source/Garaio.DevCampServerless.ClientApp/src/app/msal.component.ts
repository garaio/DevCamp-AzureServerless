import { Component} from '@angular/core';
import { MsalService } from '@azure/msal-angular';

// This component is used only to avoid Angular reload
// when doing acquireTokenSilent()
// Source: https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-js-avoid-page-reloads#initialization-in-your-main-app-file

@Component({
  selector: 'app-root',
  template: '',
})
export class MsalComponent {
  constructor(msal: MsalService) {
      console.log('Token generation in hidden iframe with authority ' + msal.authority);
  }
}
