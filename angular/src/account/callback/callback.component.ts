import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MezonLoginService } from '../../app/service/mezon-login-service/mezon-login.service';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private mezonService: MezonLoginService,
    private authService:AppAuthService
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const code = params['code'];
      if (code) {
        this.mezonService.mezonAuthenticate(code).subscribe(
          (result: any) => {
            if (result.success) {
              this.authService.processAuthenticateResult(result.result);
            }
            this.router.navigate(['account/login']);
          },
          () => {
            this.router.navigate(['account/login']);
          }
        );
      } else {
        this.router.navigate(['account/login']);
      }
    });
  }

}
