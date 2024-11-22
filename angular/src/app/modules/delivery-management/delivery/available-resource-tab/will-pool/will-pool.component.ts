import { Component, Injector, OnInit } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';

@Component({
  selector: 'app-will-pool',
  templateUrl: './will-pool.component.html',
  styleUrls: ['./will-pool.component.css']
})
export class WillPoolComponent extends PagedListingComponentBase<any> implements OnInit {
  Resource_TabWillPool = PERMISSIONS_CONSTANT.Resource_TabWillPool;
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    throw new Error('Method not implemented.');
  }
  protected delete(entity: WillPoolComponent): void {
    throw new Error('Method not implemented.');
  }

  constructor(public injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

}
