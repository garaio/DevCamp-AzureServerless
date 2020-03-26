export const environment = {
  production: true,
  apiBaseUrl: (<any>window).api.baseUrl,
  apiAuthCode: (<any>window).api.authCode,
  authClientId: (<any>window).auth.clientId,
  authLoginAuthority: (<any>window).auth.loginAuthority,
  authResetAuthority: (<any>window).auth.resetAuthority,
  authChangeAuthority: (<any>window).auth.changeAuthority,
  authScopes: (<any>window).auth.scopes,
  authRedirectUri: (<any>window).auth.redirectUri
};
