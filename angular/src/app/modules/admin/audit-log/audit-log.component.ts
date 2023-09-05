import { Component, Injector, OnInit } from '@angular/core';
import { FilterDto, PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { AuditlogService } from '../../../service/api/audit-log.service';

@Component({
  selector: 'app-audit-log',
  templateUrl: './audit-log.component.html',
  styleUrls: ['./audit-log.component.css']
})
export class AuditLogComponent extends PagedListingComponentBase<AuditlogsDto> implements OnInit {

  auditlogs = [] as AuditlogsDto[];
  emailAddressFilter = [];
  emailAddress = [];
  selecteduserId: string = "";
  emailAddressSearch = "";

  iconCondition: string = "transactionDate";
  sortDrirect: number = 0;
  transDate: string = "";
  iconSort: string = "";
  public isLoading:boolean = false;

  constructor(
    private auditlog: AuditlogService,
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getEmailAddress();
    this.refresh();
  }

  sortOrderBy(data) {
    if (this.iconCondition !== data) {
      this.sortDrirect = -1;
    }
    this.iconCondition = data;
    this.transDate = data;
    this.sortDrirect++;
    if (this.sortDrirect > 1) {
      this.transDate = "";
      this.iconSort = "";
      this.sortDrirect = -1;
    }
    if (this.sortDrirect == 1) {
      this.iconSort = "fas fa-sort-amount-down";
    } else if (this.sortDrirect == 0) {
      this.iconSort = "fas fa-sort-amount-up";
    } else {
      this.iconSort = "fas fa-sort";
    }
    this.refresh();
  }

  getEmailAddress() {
    this.auditlog.getAllEmailAddressInAuditLog().subscribe(data => {
      this.emailAddress  = this.emailAddressFilter = data.result
    })
  }
  handleSearch() {
    const textSearch = this.emailAddressSearch.toLowerCase().trim();
    if (textSearch) {
      this.emailAddress = this.emailAddressFilter
      .filter(item => item.emailAddress.toLowerCase().trim().includes(textSearch));
    } else {
      this.emailAddress = _.cloneDeep(this.emailAddressFilter);
    }
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let filterItems: FilterDto[] = [];
    request.sort = this.transDate;
    request.sortDirection = this.sortDrirect;
    if(this.selecteduserId &&this.selecteduserId != 'null'){
      filterItems.push({
        comparision: 0,
        propertyName: "userId",
        value: this.selecteduserId
      })
    }
    if(this.selecteduserId == 'null'){
      filterItems.push({
        comparision: 0,
        propertyName: "userId",
        value: null
      })
    }

    request.filterItems = filterItems;
    if (this.searchText) {
      request.searchText = this.searchText;
    }
    this.isLoading = true;
    this.auditlog
      .getAllAuditLogs(request)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result: any) => {
        this.auditlogs = result.result.items;
        for (let i = 0; i < this.auditlogs.length; i++) {
          this.auditlogs[i].hideNote = false;
        }
        this.showPaging(result.result, pageNumber);
        this.isLoading = false;
      },()=> this.isLoading = false);

  }

  changeStatusNote(item) {
    item.hideNote = !item.hideNote;
  }

  protected delete(item: AuditlogsDto): void {
  }
}

export class AuditlogsDto {
  executionDuration: number;
  executionTime: string;
  methodName: string;
  parameters: string;
  serviceName: string;
  emailAddress: string;
  userId: number;
  note: string;
  hideNote: boolean;
}
