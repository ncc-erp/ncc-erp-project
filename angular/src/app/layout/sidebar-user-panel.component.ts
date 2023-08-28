import {
  Component,
  ChangeDetectionStrategy,
  Injector,
  OnInit
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '@app/service/api/user.service';
import { UploadAvatarComponent } from '@app/users/upload-avatar/upload-avatar.component';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { UserDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'sidebar-user-panel',
  templateUrl: './sidebar-user-panel.component.html',
  // changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarUserPanelComponent extends AppComponentBase
  implements OnInit {
  shownLoginName = '';
    users:UserDto[]=[]
    currentUser
  constructor(injector: Injector, private userService:UserService, private dialog:MatDialog) {
    super(injector);
  }

  ngOnInit() {
    this.shownLoginName = this.appSession.getShownLoginName();
    this.currentUser = this.appSession.user
  }
  updateAvatar(num): void {
    // var diaLogRef = this.dialog.open(UploadAvatarComponent, {
    //     width: "600px",
    //     data: num
    // });
    // diaLogRef.afterClosed().subscribe(res => {
    //     if (res) {
    //         this.userService.upLoadOwnAvatar(res).subscribe(data => {
    //             if (data) {
    //                 this.notify.success("Upload Avatar Successfully!");
    //                 this.appSession.user.avatarPath = data.body.result;
    //             } else this.notify.error("Upload Avatar Failed!");
    //         });
    //     }
    // });
}

}
