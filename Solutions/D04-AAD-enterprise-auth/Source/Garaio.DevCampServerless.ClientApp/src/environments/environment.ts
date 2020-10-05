// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiBaseUrl: 'https://ga-dcs-service-f-aad.azurewebsites.net/api',
  apiAuthCode: '00000000-0000-0000-0000-000000000000',
  authClientId: '9317ca9f-0000-0000-0000-9b086409e308',
  authLoginAuthority: 'https://login.microsoftonline.com/3b930b5c-0000-0000-0000-53fcdf8762e8',
  authResetAuthority: 'https://login.microsoftonline.com/3b930b5c-0000-0000-0000-53fcdf8762e8',
  authChangeAuthority: 'https://login.microsoftonline.com/3b930b5c-0000-0000-0000-53fcdf8762e8',
  authGeneralScopes: ['openid', 'user.read'],
  authApiScopes: ['api://a1c5f540-0000-0000-0000-8a7b1b00229b/user_impersonation'],
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
