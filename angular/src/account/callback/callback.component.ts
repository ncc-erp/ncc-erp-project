import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MezonLoginService } from '../../app/service/mezon-login-service/mezon-login.service';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppComponentBase } from '@shared/app-component-base';
import { LoginService } from 'account/login/login.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent extends AppComponentBase implements OnInit {

  constructor(
    injector: Injector,
    // private route: ActivatedRoute,
    // private router: Router,
    private mezonService: MezonLoginService,
    private authService: AppAuthService,
    public loginService: LoginService,
  ) {
    super(injector)
  }

  ngOnInit(): void {
    // this.route.queryParams.subscribe(params => {
    //   const code = params['code'];
    //   if (code) {
    //     this.mezonService.mezonAuthenticate(code).subscribe(
    //       (result: any) => {
    //         if (result.success) {
    //           this.authService.processAuthenticateResult(result.result);
    //         }
    //         this.router.navigate(['account/login']);
    //       },
    //       () => {
    //         this.router.navigate(['account/login']);
    //       }
    //     );
    //   } else {
    //     this.router.navigate(['account/login']);
    //   }
    // });

    this.routeBase.queryParams.subscribe(params => {
      this.isLoading = true;
      const code = params['code'];
      const scope = params['scope'];
      const state = params['state'];
      // if (code && scope && state) {
      //   this.showToastMessage(ToastMessageType.ERROR, 'something went wrong!');
      // }

      this.loginService.authenticateMezon(code, scope).subscribe(res => {
        this.isLoading = res.loading;
      });
    });
  }

}
