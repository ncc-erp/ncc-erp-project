import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectResourceService } from '@app/service/api/project-resource.service';
import { ProjectResourceDto } from '@app/service/model/list-project.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-edit-project-resource',
  templateUrl: './create-edit-project-resource.component.html',
  styleUrls: ['./create-edit-project-resource.component.css']
})
export class CreateEditProjectResourceComponent extends AppComponentBase implements OnInit {
  public title = '';
  public item = {} as ProjectResourceDto;
  public parentList: ProjectResourceDto[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateEditProjectResourceComponent>,
    public projectResourceService: ProjectResourceService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.item = { ...this.data.item };
    this.parentList = this.data.parentList || [];
    this.title = this.data.command === 'create' ? 'Create Project Resource' : 'Edit Project Resource';
  }

  public save(): void {
    const payload = {
      id: this.item.id,
      name: this.item.name,
      note: this.item.note,
      parentId: this.item.parentId || null
    } as ProjectResourceDto;

    const request = this.data.command === 'create'
      ? this.projectResourceService.create(payload)
      : this.projectResourceService.update(payload);

    request.pipe(catchError(this.projectResourceService.handleError)).subscribe(() => {
      abp.notify.success(this.data.command === 'create' ? 'Create Project Resource Successfully!' : 'Update Project Resource Successfully!');
      this.dialogRef.close(true);
    }, () => {
      this.isLoading = false;
    });
  }
}
