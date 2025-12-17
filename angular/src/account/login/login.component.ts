import { Component, ElementRef, Injector, ViewChild } from "@angular/core";
import { AbpSessionService } from "abp-ng2-module";
import { AppComponentBase } from "@shared/app-component-base";
import { accountModuleAnimation } from "@shared/animations/routerTransition";
import { AppAuthService } from "@shared/auth/app-auth.service";
import { SocialUser } from "angularx-social-login";
import { IHashMezonAuthModel, LoginService } from "./login.service";
import { MezonLoginService } from "@app/service/mezon-login-service/mezon-login.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  templateUrl: "./login.component.html",
  animations: [accountModuleAnimation()],
})
export class LoginComponent extends AppComponentBase {
  submitting = false;
  user: SocialUser;
  tenancyName: string;
  loggedIn: boolean;
  hashData: string;
  accessFromMezonApp: boolean = false;

  constructor(
    injector: Injector,
    public authService: AppAuthService,
    private _sessionService: AbpSessionService,
    private _loginService: LoginService,
    private _activatedRoute: ActivatedRoute,
    public mezonLoginService: MezonLoginService
  ) {
    super(injector);
  }
  ngOnInit(): void {

    this._activatedRoute.queryParams.subscribe((params) => {
      if (params["data"]) {
        this.hashData = params["data"];
        this.accessFromMezonApp = true;
        this.signInWithHash(this.hashData);
        return;
      }
    });

    const searchParams = new URLSearchParams(window.location.search);
    const rootData = searchParams.get("data");
    if (rootData) {
      this.signInWithHash(rootData);
      return;
    }
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

  signInWithHash(hash: string) {
    if (hash) {
      const hashData: IHashMezonAuthModel = {
        hashData: btoa(hash),
        tenancyName: null,
      };

      this._loginService.authenticateMezonHash(hashData).subscribe(
        (data) => (this.isLoading = data.isLoading),
        (err) => {
          abp.notify.error("Please try to login again!");
          this.isLoading = false;
        }
      );
    }
  }

  signInWithMezon() {
    this.mezonLoginService.redirectToOAuth();
  }
}
