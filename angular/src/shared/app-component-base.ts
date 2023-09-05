import { Utils } from './Utils';
import { Injector, ElementRef } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import {
    LocalizationService,
    PermissionCheckerService,
    FeatureCheckerService,
    NotifyService,
    SettingService,
    MessageService,
    AbpMultiTenancyService
} from 'abp-ng2-module';

import { AppSessionService } from '@shared/session/app-session.service';
import { APP_ENUMS } from './AppEnums';
import * as moment from 'moment';

export abstract class AppComponentBase {

    APP_ENUM = APP_ENUMS;
    APP_CONST = AppConsts;
    Utils = Utils;
    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

    localization: LocalizationService;
    permission: PermissionCheckerService;
    feature: FeatureCheckerService;
    notify: NotifyService;
    setting: SettingService;
    message: MessageService;
    multiTenancy: AbpMultiTenancyService;
    appSession: AppSessionService;
    elementRef: ElementRef;
    isLoading: boolean = false;
    checkNull: boolean = false;

    constructor(injector: Injector) {
        this.localization = injector.get(LocalizationService);
        this.permission = injector.get(PermissionCheckerService);
        this.feature = injector.get(FeatureCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.multiTenancy = injector.get(AbpMultiTenancyService);
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
    }

    l(key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, this.localizationSourceName);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }
    public getByEnum(enumValue: number, enumObject: any) {
        for (const key in enumObject) {
          if (enumObject[key] == enumValue) {
            return key;
          }
        }
    }
    public enterName(name : string) : any{
      const pattern = '^[_ ]*$';
      if(name.match(pattern)){
        this.checkNull = true;
      }else{
        this.checkNull = false;
      }
    }
    public formatDateYMD(date:any){
        return moment(date).format("YYYY-MM-DD")
    }
    getAvatar(member) {
      if (member.avatarFullPath) {
        return member.avatarFullPath;
      }
      if (member.fullAvatarPath) {
        return member.fullAvatarPath;
      }
      return '/assets/img/user.png';
    }
}
