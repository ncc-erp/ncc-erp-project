import { TalentService } from './../../../../../service/api/talent.service';
import { PosistionTalentDto, SendRecuitmentDto, BranchTalentDto } from './../../../../../service/model/talent.dto';
import { SendRecruitmentModel } from './../request-resource-tab.component';
import { AppComponentBase } from 'shared/app-component-base';
import { ChangeDetectionStrategy, Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';

@Component({
  selector: 'app-form-send-recruitment',
  templateUrl: './form-send-recruitment.component.html',
  styleUrls: ['./form-send-recruitment.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormSendRecruitmentComponent extends AppComponentBase implements OnInit {
  branches: BranchTalentDto[] = [];
  positions: PosistionTalentDto[] = [];
  recruitment = new SendRecuitmentDto();
  isSending: boolean = false;
  searchSubPosition: string;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: SendRecruitmentModel,
    private _resourceRequestService: DeliveryResourceRequestService,
    public injector: Injector,
    public dialogRef: MatDialogRef<FormSendRecruitmentComponent>,
    private _talent: TalentService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.setRecruitment();
    this.getUserTypes();
    this.getPositions();
  }
  private setRecruitment() {
    this.recruitment.resourceRequestId = this.data.id;
    this.recruitment.note = (this.data?.pmNote ?? '') + '\n' + (this.data.dmNote ?? '');
  }
  getUserTypes() {
    this._talent.getBranches().subscribe((rs) => {
      if (!rs.success) return;
      this.branches = rs.result;
    });
  }
  getPositions() {
    this._talent.getPositions().subscribe((rs) => {
      if (!rs.success) return;
      this.positions = rs.result;
    })
  }
  save() {
    this.isSending = true;
    this._talent.sendRecruitment(this.recruitment).subscribe(
      (rs) => {
        abp.notify.success("Sent Recruitment Successfully!");
        this.dialogRef.close(rs.result);
      },
      (err) => {
        abp.notify.error(err);
        this.isSending = false;
      }
    )
  }
}
