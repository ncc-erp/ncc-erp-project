import { Component, Injector, OnInit } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';

@Component({
  selector: 'app-backup-contribution',
  templateUrl: './backup-contribution.component.html',
  styleUrls: ['./backup-contribution.component.css']
})
export class BackupContributionComponent extends PagedListingComponentBase<BackupContributionComponent> implements OnInit {
  public Admin_Backup:string = PERMISSIONS_CONSTANT.Admin_Backup;

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    throw new Error('Method not implemented.');
  }
  
  protected delete(entity: BackupContributionComponent): void {
    throw new Error('Method not implemented.');
  }

  constructor(injector: Injector) { super(injector) }

  ngOnInit(): void {
  }

}
