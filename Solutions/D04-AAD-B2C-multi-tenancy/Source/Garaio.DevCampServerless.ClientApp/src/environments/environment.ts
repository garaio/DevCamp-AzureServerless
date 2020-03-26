// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiBaseUrl: 'https://ga-dcs-service-f-<suffix>.azurewebsites.net/api',
  apiAuthCode: '00000000-0000-0000-0000-000000000000',
  authClientId: '00000000-0000-0000-0000-000000000000',
  authLoginAuthority: 'https://gadcs<suffix>.b2clogin.com/gadcs<suffix>.onmicrosoft.com/B2C_1_signupsignin',
  authResetAuthority: 'https://gadcs<suffix>.b2clogin.com/gadcs<suffix>.onmicrosoft.com/B2C_1_passwordreset',
  authChangeAuthority: 'https://gadcs<suffix>.b2clogin.com/gadcs<suffix>.onmicrosoft.com/B2C_1_profileediting',
  authScopes: ['https://gadcs<suffix>.onmicrosoft.com/ServiceFuncApp/access'],
  authRedirectUri: 'http://localhost:4200'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
