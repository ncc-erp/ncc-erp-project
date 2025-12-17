import { Injectable } from "@angular/core";
import { PermissionCheckerService } from "abp-ng2-module";
import { AppSessionService } from "../session/app-session.service";

import {
  CanActivate,
  Router,
  ActivatedRoute,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from "@angular/router";

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {
  constructor(
    private _permissionChecker: PermissionCheckerService,
    private _router: Router,
    private _sessionService: AppSessionService,
    private _activatedRoute: ActivatedRoute
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {

    if (!this._sessionService.user) {

      let dataParam = route.queryParamMap.get('data');

      if (!dataParam) {
        const urlParams = new URLSearchParams(window.location.search);
        dataParam = urlParams.get('data');
      }

      if (dataParam) {
        this._router.navigate(["/account/login"], {
          queryParams: { data: dataParam },
        });
      } else {
        this._router.navigate(["/account/login"]);
      }

      return false;
    }

    if (!route.data || !route.data["permission"]) {
      return true;
    }

    if (this._permissionChecker.isGranted(route.data["permission"])) {
      return true;
    }

    this._router.navigate([this.selectBestRoute()]);
    return false;
  }

  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    return this.canActivate(route, state);
  }

  selectBestRoute(): string {
    if (!this._sessionService.user) {
      return "/account/login";
    }

    if (this._permissionChecker.isGranted("Pages.Users")) {
      return "/app/admin/users";
    }
    return "/app/home";
  }
}