// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiBaseUrl: 'https://ga-dcs-service-f-b2c.azurewebsites.net/api',
  apiAuthCode: 'bada7656-1856-47b7-b0e8-c3c48d7a8e3d',
  authClientId: 'fab71150-01f7-4e06-a951-456e520e8833',
  authLoginAuthority: 'https://gadcsb2c.b2clogin.com/gadcsb2c.onmicrosoft.com/B2C_1_signupsignin',
  authResetAuthority: 'https://gadcsb2c.b2clogin.com/gadcsb2c.onmicrosoft.com/B2C_1_passwordreset',
  authChangeAuthority: 'https://gadcsb2c.b2clogin.com/gadcsb2c.onmicrosoft.com/B2C_1_profileediting',
  authScopes: ['https://gadcsb2c.onmicrosoft.com/ServiceFuncApp/access'],
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
