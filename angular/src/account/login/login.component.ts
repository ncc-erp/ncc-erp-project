import { Component, Injector } from '@angular/core';
import { AbpSessionService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';
import { IHashMezonAuthModel, LoginService } from './login.service';
import { MezonLoginService } from '@app/service/mezon-login-service/mezon-login.service';
@Component({
  templateUrl: './login.component.html',
  animations: [accountModuleAnimation()]
})
export class LoginComponent extends AppComponentBase {
  submitting = false;
  user: SocialUser
  tenancyName: string
  loggedIn: boolean;
  hashData: string;

  constructor(
    injector: Injector,
    public authService: AppAuthService,
    private _sessionService: AbpSessionService,
    private googleAuthService: SocialAuthService,
    private loginService: LoginService,
    public mezonLoginService: MezonLoginService
  ) {
    super(injector);
  }
  ngOnInit(): void {
    // this.googleAuthService.authState.subscribe((user) => {
    //   this.user = user;
    //   this.loggedIn = (user != null);
    //   if (this.loggedIn) {
    //     this.loginService.authenticateGoogle(this.user.idToken);
    //  }
    // });

    this.mezonLoginService.userHashData$.subscribe((userHashData) => {
      this.isLoading = true;
      this.hashData = userHashData;
      this.loginWithHash(this.hashData);
    });
  }
  get multiTenancySideIsTeanant(): boolean {
    return this._sessionService.tenantId > 0;
  }

  get isSelfRegistrationAllowed(): boolean {
    if (!this._sessionService.tenantId) {
      return false;
    }

    return true;
  }
  login(): void {
    this.submitting = true;
    this.authService.authenticate(() => (this.submitting = false));
  }
  // signInWithGoogle() {
  //   this.googleAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  // }

  loginWithHash(hash: string) {
    if (hash) {
      const hashData: IHashMezonAuthModel = {
        hashData: btoa(hash),
        tenancyName: null
      };

      this.loginService.authenticateMezonHash(hashData).subscribe(data => this.isLoading = data.isLoading);
    }
  }

  signInWithMezon() {
    this.mezonLoginService.redirectToOAuth();
  }
}
