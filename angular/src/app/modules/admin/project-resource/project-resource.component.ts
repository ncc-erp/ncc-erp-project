import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProjectResourceService } from '@app/service/api/project-resource.service';
import { ProjectResourceDto } from '@app/service/model/list-project.dto';
import { CreateEditProjectResourceComponent } from './create-edit-project-resource/create-edit-project-resource.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-project-resource',
  templateUrl: './project-resource.component.html',
  styleUrls: ['./project-resource.component.css']
})
export class ProjectResourceComponent extends AppComponentBase implements OnInit {
  public resources: ProjectResourceDto[] = [];
  public searchText = '';
  public expandedIds = new Set<number>();

  constructor(
    private projectResourceService: ProjectResourceService,
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
    this.loadResources();
  }

  public loadResources(): void {
    this.projectResourceService.getAll(this.searchText || undefined).subscribe((res) => {
      this.resources = res.result || res;
      this.expandedIds.clear();
    });
  }

  public hasChildren(node: ProjectResourceDto): boolean {
    return !!node.childrens && node.childrens.length > 0;
  }

  public toggleNode(node: ProjectResourceDto): void {
    if (!this.hasChildren(node)) {
      return;
    }

    if (this.expandedIds.has(node.id)) {
      this.expandedIds.delete(node.id);
    } else {
      this.expandedIds.add(node.id);
    }
  }

  public isExpanded(node: ProjectResourceDto): boolean {
    return this.expandedIds.has(node.id) || !this.hasChildren(node);
  }

  public createProjectResource(parent?: ProjectResourceDto): void {
    const dialogRef = this.dialog.open(CreateEditProjectResourceComponent, {
      data: { command: 'create', item: { name: '', parentId: parent ? parent.id : null }, parentList: this.resources },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.loadResources();
      }
    });
  }

  public editProjectResource(item: ProjectResourceDto): void {
    const dialogRef = this.dialog.open(CreateEditProjectResourceComponent, {
      data: { command: 'update', item: { ...item }, parentList: this.resources },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.loadResources();
      }
    });
  }

  public deleteProjectResource(item: ProjectResourceDto): void {
    if (item.childrens && item.childrens.length > 0) {
      abp.message.warn('Cannot delete a project resource that has child nodes.');
      return;
    }

    abp.message.confirm('Delete Project Resource ' + item.name + '?', '', (result: boolean) => {
      if (result) {
        this.projectResourceService.delete(item.id).subscribe(() => {
          abp.notify.success('Delete Project Resource ' + item.name);
          this.loadResources();
        });
      }
    });
  }
}
