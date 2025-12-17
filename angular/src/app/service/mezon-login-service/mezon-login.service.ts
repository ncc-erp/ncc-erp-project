import { BaseApiService } from '../api/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { AppConsts } from './../../../shared/AppConsts';
import { MezonWebViewEvent, MezonAppEvent } from '../../../../src/types/mezon/webview';

import { IHashMezonAuthModel } from '../../../account/login/login.service';

@Injectable({
  providedIn: 'root'
})
export class MezonLoginService extends BaseApiService {
  private userHashData = new Subject<string>();
  private isInMezon = new Subject<boolean>();

  private eventListenersRegistered = false;

  userHashData$ = this.userHashData.asObservable();
  isInMezon$ = this.isInMezon.asObservable();


  changeUrl() {
    return 'TokenAuth';
  }

  constructor(
    http: HttpClient
  ) {
    super(http);
  }

  name() {
    return 'TokenAuth';
  }

  public initMezonEventListeners(): void {
    if (this.eventListenersRegistered)
      return;

    this.eventListenersRegistered = true;

    if (window.Mezon && window.Mezon.WebView) {
      this.ping();
      this.sendBotId();

      this.listenToPong();
      this.listenToUserHashInfo();

    }
  }

  ping() {

    window.Mezon.WebView?.postEvent("PING" as MezonWebViewEvent, { message: "PING" }, () => { })
  }

  listenToPong() {
    window.Mezon.WebView?.onEvent("PONG" as MezonAppEvent, () => {
      this.isInMezon.next(true);
    });
  }

  sendBotId() {
    window.Mezon.WebView?.postEvent("SEND_BOT_ID" as MezonWebViewEvent, { appId: AppConsts.mezonAppId }, () => { })
  }

  listenToUserHashInfo() {
    window.Mezon.WebView?.onEvent("USER_HASH_INFO" as MezonAppEvent, async (_, userHashData: any) => {
      this.userHashData.next(userHashData.message.web_app_data);
    });
  }

  removeEventListeners() {
    window.Mezon.WebView?.offEvent("CURRENT_USER_INFO" as MezonAppEvent, () => { })
    window.Mezon.WebView?.offEvent("USER_HASH_INFO" as MezonAppEvent, () => { })
  }

  redirectToOAuth() {
    window.location.href = `${this.baseUrl}/api/TokenAuth/MezonRedirect`;
  }

  mezonAuthenticate(token: string): Observable<any> {
    return this.http.post(this.baseUrl + '/api/TokenAuth/MezonAuthenticate', { token: token });
  }

  mezonHashAuthenticate(authDto: IHashMezonAuthModel): Observable<any> {
    return this.http.post(this.baseUrl + '/api/TokenAuth/MezonHashAuthenticate', authDto);
  }
}
