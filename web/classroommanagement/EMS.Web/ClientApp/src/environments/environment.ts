// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  suite: 'SilverLeaf',
  environmentName: 'Development',
  identityServer: 'http://localhost:5000',
  clientId: 'SilverLeaf.Portal',
  websiteUrl: 'http://localhost:4200/',
  callbackRoute: 'auth-callback',

  emsApi: {
    url: 'http://localhost:3000/',
    apiExtension: 'api/',
    version: 'v1/',
    scope: 'EMS.API'
  },

  scopes: 'openid profile email roles ',
  responseType: 'id_token token',
  tokenRefreshUri: 'silent-refresh.html'
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
