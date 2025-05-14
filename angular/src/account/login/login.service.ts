import { Injectable } from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { GoogleLoginService } from '../../app/service/google-login-service/google-login.service';
import { MezonLoginService } from '../../app/service/mezon-login-service/mezon-login.service';
import { Observable, of } from '@node_modules/rxjs';
import { catchError, finalize, map, startWith } from 'rxjs/operators';
import { HttpErrorResponse } from '@node_modules/@angular/common/http';
import { AuthenticateResultModel } from '@shared/service-proxies/service-proxies';
import { TokenService, UtilsService, LogService } from 'abp-ng2-module';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
export interface IHashMezonAuthModel {
  hashData: string;
  tenancyName: string;
}
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  authenticateResult: AuthenticateResultModel;
  rememberMe: boolean;
  constructor(
    private _googleLoginService: GoogleLoginService,
    private authService: AppAuthService,
    private _mezonLoginService: MezonLoginService,
    private _tokenService: TokenService,
    private _utilsService: UtilsService,
    private _logService: LogService,
    private _router: Router,
  ) { }

  authenticateGoogle(googleToken: string, finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => { });

    this._googleLoginService.googleAuthenticate(googleToken)
      .subscribe((result: any) => {
        this.authService.processAuthenticateResult(result.result)
      });
  }

  redirectToOAuth(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => { });
    this._mezonLoginService.redirectToOAuth();
  }

  authenticateMezon(token: string, scope: string): Observable<any> {
    return this._mezonLoginService.mezonAuthenticate(token).pipe(
      map(data => {
        var result = this.processAuthenticateResult(data.result);
        return { ...data, loading: false }
      }),
      startWith({ loading: true, success: false }),
      catchError((err: HttpErrorResponse) => {
        return of({ loading: false, success: false, error: err.error.error });
      }),
    );
  }

  authenticateMezonHash(authDto: IHashMezonAuthModel): Observable<any> {
    return this._mezonLoginService.mezonHashAuthenticate(authDto).pipe(
      map(data => {
        this.processAuthenticateResult(data.result);
        return { ...data, loading: false }
      }),
      startWith({ loading: true, success: false }),
      catchError((err: HttpErrorResponse) => {
        return of({ loading: false, success: false, error: err.error.error });
      }),
    );
  }

  private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
    this.authenticateResult = authenticateResult;

    if (authenticateResult.accessToken) {
      // Successfully logged in
      this.login(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        this.rememberMe);

    } else {
      // Unexpected result!
      this._logService.warn('Unexpected authenticateResult!');
      this._router.navigate(['account/login']);
    }
  }

  private login(accessToken: string, encryptedAccessToken: string, expireInSeconds: number, rememberMe?: boolean): void {

    const tokenExpireDate = rememberMe ? (new Date(new Date().getTime() + 1000 * expireInSeconds)) : undefined;
    this._tokenService.setToken(
      accessToken,
      tokenExpireDate
    );

    this._utilsService.setCookieValue(
      AppConsts.authorization.encryptedAuthTokenName,
      encryptedAccessToken,
      tokenExpireDate,
      abp.appPath
    );

    let initialUrl = UrlHelper.initialUrl;
    if (initialUrl.indexOf('/login') > 0) {
      initialUrl = AppConsts.appBaseUrl;
    }

    location.href = initialUrl;
  }
}
