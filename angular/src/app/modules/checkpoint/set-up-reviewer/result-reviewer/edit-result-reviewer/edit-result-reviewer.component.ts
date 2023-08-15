import { TagsService } from './../../../../../service/api/tags.service';
import { catchError } from 'rxjs/operators';
import { APP_ENUMS } from './../../../../../../shared/AppEnums';
import { CheckpointUserResultService } from './../../../../../service/api/checkpoint-user-result.service';
import { CheckpointResultDto, CheckpointUserEditDto } from './../../../../../service/model/result-review.dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from 'shared/app-component-base';
import { Component, OnInit, inject, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-edit-result-reviewer',
  templateUrl: './edit-result-reviewer.component.html',
  styleUrls: ['./edit-result-reviewer.component.css']
})
export class EditResultReviewerComponent extends AppComponentBase implements OnInit {
  public reviewer={} as CheckpointResultDto;
  public listTags=[];
  public listNowLevel: String[]=Object.keys(APP_ENUMS.UserLevel);

  constructor(@Inject(MAT_DIALOG_DATA)public data:any,
    public injector: Injector,
    public dialogRef:MatDialogRef<EditResultReviewerComponent>,
    public checkpointUserResult: CheckpointUserResultService,
    public tabService: TagsService) {
    super(injector);
  }

  ngOnInit(): void {
    this.reviewer=this.data.item;
    console.log(this.reviewer)
    this.getAllTag();
  }
  SaveAndClose(){
    let reviewerEdit={
      checkPointUserResultId: this.reviewer.id,
      finalNote: this.reviewer.finalNote,
      now: this.reviewer.nowLevel,
      tagIds:this.reviewer.tag
    }
    this.checkpointUserResult.EditMain(reviewerEdit).pipe(catchError(this.checkpointUserResult.handleError)).subscribe((res)=>{
      abp.notify.success("Edit Successful!");
      this.reviewer={
        id:reviewerEdit.checkPointUserResultId,
        finalNote: reviewerEdit.finalNote,
        nowLevel: reviewerEdit.now,
        tag:reviewerEdit.tagIds
        }
        
        console.log(this.reviewer)
      this.dialogRef.close(this.reviewer);

    })
    
  }
  getAllTag(){
    this.tabService.getAll().subscribe((data)=>{
      this.listTags=data.result;
    })
  }
  list=[
    {
      "name": "Up",
      "id": 1
    },
    {
      "name": "Talk",
      "id": 3
    }
  ]
  onTypeChange(){
    

  }

}
