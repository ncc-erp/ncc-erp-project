import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { CreateUpdateTechnologyComponent } from './create-update-technology/create-update-technology.component';
import { MatDialog } from '@angular/material/dialog';
import { finalize, catchError } from 'rxjs/operators';
import { TechnologyDto } from '../../../service/model/technology.dto'; 
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { TechnologyService } from '../../../service/api/technology.service';
import { Component, OnInit, Injector } from '@angular/core';
import { InputFilterDto } from '@shared/filter/filter.component';

@Component({
  selector: 'app-technology',
  templateUrl: './technology.component.html',
  styleUrls: ['./technology.component.css']
})

export class TechnologyComponent extends PagedListingComponentBase<TechnologyComponent> implements OnInit {

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.technologyService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.technologyService.handleError)).subscribe(data => {
      this.technologyList = data.result.items;
      this.showPaging(data.result, pageNumber)
    })
  }

  protected delete(technology: TechnologyComponent): void {
    abp.message.confirm(
      "Delete technology " + technology.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.technologyService.deleteTechnology(technology.id).pipe(catchError(this.technologyService.handleError)).subscribe((res) => {
            abp.notify.success("Delele technology " + technology.name);
            this.refresh()
          })
        }
      }
    )
  }

  public technologyList: TechnologyDto[] = [];
  Admin_Technologies_View = PERMISSIONS_CONSTANT.Admin_Technologies_View;
  Admin_Technologies_Create = PERMISSIONS_CONSTANT.Admin_Technologies_Create;
  Admin_Technologies_Edit = PERMISSIONS_CONSTANT.Admin_Technologies_Edit;
  Admin_Technologies_Delete = PERMISSIONS_CONSTANT.Admin_Technologies_Delete;

  constructor(private technologyService: TechnologyService,
    injector: Injector,
    private dialog: MatDialog) { super(injector) }

  ngOnInit(): void {
    this.refresh();
  }
  
  public showDialog(command: string, technology: any) {
    let item = {
      name: technology.name,
      code: technology.code,
      displayName: technology.displayName,
      color: technology.color,
      id: technology.id,
    }
    const show = this.dialog.open(CreateUpdateTechnologyComponent, {
      data: {
        item: item,
        command: command
      },
      width: "700px"
    })
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    })
  }

  public createTechnology() {
    this.showDialog("create", {});
  }
  public editTechnology(technology) {
    this.showDialog("update", technology);
  }
}
