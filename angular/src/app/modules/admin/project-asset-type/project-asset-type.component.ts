import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProjectAssetTypeService } from '@app/service/api/project-asset-type.service';
import { ProjectAssetTypeDto } from '@app/service/model/list-project.dto';
import { CreateEditProjectAssetTypeComponent } from './create-edit-project-asset-type/create-edit-project-asset-type.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-project-asset-type',
  templateUrl: './project-asset-type.component.html',
  styleUrls: ['./project-asset-type.component.css']
})
export class ProjectAssetTypeComponent extends AppComponentBase implements OnInit {
  public resources: ProjectAssetTypeDto[] = [];
  public searchText = '';
  public expandedIds = new Set<number>();

  constructor(
    private projectAssetTypeService: ProjectAssetTypeService,
    private dialog: MatDialog,
    injector: Injector
  ) {
    super(injector);
  }

  public Admin_ProjectAssetTypes_View = PERMISSIONS_CONSTANT.Admin_ProjectAssetTypes_View;
  public Admin_ProjectAssetTypes_Create = PERMISSIONS_CONSTANT.Admin_ProjectAssetTypes_Create;
  public Admin_ProjectAssetTypes_Edit = PERMISSIONS_CONSTANT.Admin_ProjectAssetTypes_Edit
  public Admin_ProjectAssetTypes_Delete = PERMISSIONS_CONSTANT.Admin_ProjectAssetTypes_Delete;
  public Admin_ProjectAssetTypes = PERMISSIONS_CONSTANT.Admin_ProjectAssetTypes;

  ngOnInit(): void {
    this.loadAssetTypes();
  }

  public loadAssetTypes(): void {
    this.projectAssetTypeService.getAll(this.searchText || undefined).subscribe((res) => {
      this.resources = res.result || res;
      this.expandedIds.clear();
    });
  }

  public hasChildren(node: ProjectAssetTypeDto): boolean {
    return !!node.childrens && node.childrens.length > 0;
  }

  public toggleNode(node: ProjectAssetTypeDto): void {
    if (!this.hasChildren(node)) {
      return;
    }

    if (this.expandedIds.has(node.id)) {
      this.expandedIds.delete(node.id);
    } else {
      this.expandedIds.add(node.id);
    }
  }

  public isExpanded(node: ProjectAssetTypeDto): boolean {
    return this.expandedIds.has(node.id) || !this.hasChildren(node);
  }

  public createProjectAssetType(parent?: ProjectAssetTypeDto): void {
    const dialogRef = this.dialog.open(CreateEditProjectAssetTypeComponent, {
      data: { command: 'create', item: { name: '', parentId: parent ? parent.id : null }, parentList: this.resources },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.loadAssetTypes();
      }
    });
  }

  public editProjectAssetType(item: ProjectAssetTypeDto): void {
    const dialogRef = this.dialog.open(CreateEditProjectAssetTypeComponent, {
      data: { command: 'update', item: { ...item }, parentList: this.resources },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.loadAssetTypes();
      }
    });
  }

  public deleteProjectAssetType(item: ProjectAssetTypeDto): void {
    if (item.childrens && item.childrens.length > 0) {
      abp.message.warn('Cannot delete a project asset type that has child nodes.');
      return;
    }

    abp.message.confirm('Delete Project Asset Type ' + item.name + '?', '', (result: boolean) => {
      if (result) {
        this.projectAssetTypeService.delete(item.id).subscribe(() => {
          abp.notify.success('Delete Project Asset Type ' + item.name);
          this.loadAssetTypes();
        });
      }
    });
  }
}
