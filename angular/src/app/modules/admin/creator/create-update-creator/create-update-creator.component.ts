import { CreatorManagerService } from './../../../../service/api/creator-manager.service';
import { CreatorDto } from './../../../../service/model/list-project.dto';
import { Inject, Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-create-update-creator',
  templateUrl: './create-update-creator.component.html',
  styleUrls: ['./create-update-creator.component.css']
})
export class CreateUpdateCreatorComponent extends AppComponentBase implements OnInit {
  public title = '';
  public creator = {} as CreatorDto;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public creatorService: CreatorManagerService,
    public dialogRef: MatDialogRef<CreateUpdateCreatorComponent>) { super(injector); }

  ngOnInit(): void {
    this.creator = this.data.item;
    this.title = this.creator.name;
  }

  SaveAndClose() {
    if (this.data.command === 'create') {
      this.creatorService.create(this.creator).pipe(catchError(this.creatorService.handleError)).subscribe(() => {
        abp.notify.success('Create Creator Successfully!');
        this.dialogRef.close(this.creator);
      }, () => { this.isLoading = false; });
    } else {
      this.creatorService.update(this.creator).pipe(catchError(this.creatorService.handleError)).subscribe(() => {
        abp.notify.success('Update Creator Successfully!');
        this.dialogRef.close(this.creator);
      }, () => { this.isLoading = false; });
    }
  }
}
