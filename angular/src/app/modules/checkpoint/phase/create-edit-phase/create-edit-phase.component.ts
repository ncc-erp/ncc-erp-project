import { catchError } from 'rxjs/operators';
import { result } from 'lodash-es';
import { PhaseDto } from './../../../../service/model/phase.dto';
import { AppComponentBase } from 'shared/app-component-base';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, inject, Injector, Inject } from '@angular/core';
import { PhaseService } from '@app/service/api/phase.service';

@Component({
  selector: 'app-create-edit-phase',
  templateUrl: './create-edit-phase.component.html',
  styleUrls: ['./create-edit-phase.component.css']
})
export class CreateEditPhaseComponent extends AppComponentBase implements OnInit {
  public phase = {} as PhaseDto;
  public listYear: number[] = [];
  private currentYear = new Date().getFullYear();
  public parenttList = [];
  public tempparenttList = [];
  typePhase = Object.keys(this.APP_ENUM.TypePhase);
  public check: boolean;


  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CreateEditPhaseComponent>,
    public injector: Injector,
    public phaseSerivce: PhaseService) { super(injector) }

  ngOnInit(): void {
    this.phase = this.data.item;
    console.log(this.data.item);
    for (let i = this.currentYear - 4; i < this.currentYear + 2; i++) {
      this.listYear.push(i)
    }
    if (this.phase.type == '0') {
      this.phase.isCriteria = true;
    } if (this.phase.type == '1') {
      this.phase.isCriteria = false;
    }
    
    if (this.data.command == 'edit') {
      this.getParent(this.phase.year);
    }



  }
  public getParent(year) {
    this.phaseSerivce.getParent(this.phase.year).subscribe((data) => {
      this.parenttList = data.result;
      this.tempparenttList = this.parenttList;

    })

  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  onTypeChange() {
    if (this.phase.type == '0') {
      this.phase.isCriteria = true;
      this.parenttList = [];
    } if (this.phase.type == '1') {
      this.phase.isCriteria = false;
      this.parenttList = this.tempparenttList;
    }
  }
  SaveAndClose() {
    if (!this.phase.status) {
      this.phase.status = 1;
    }
    if (this.data.command == "create") {

   
      this.phase.index = 0;

      this.phaseSerivce.create(this.phase).pipe(catchError(this.phaseSerivce.handleError)).subscribe((res) => {
        abp.notify.success("Create phase"+this.phase.name +" successfully");
        this.dialogRef.close(this.phase);
      });
      // 
    }
    else {
      this.phaseSerivce.update(this.phase).pipe(catchError(this.phaseSerivce.handleError)).subscribe((res) => {
        abp.notify.success("Phase "+this.phase.name+" has been edited successfully");
        this.dialogRef.close(this.phase);
      });
    }
    console.log(this.phase);
  }
  changeStatus(e) {

    if (e.checked == true) {
      this.phase.status = 0;
    } else {
      this.phase.status = 1;
    }
    console.log(this.phase)
  }

}
