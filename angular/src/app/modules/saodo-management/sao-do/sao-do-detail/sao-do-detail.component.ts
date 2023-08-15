import { result } from 'lodash-es';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AuditResultService } from './../../../../service/api/auditresult.service';
import { CreateEditSaoDoProjectComponent } from './create-edit-sao-do-project/create-edit-sao-do-project.component';
import { SaodoDetailDto } from './../../../../service/model/saodo.dto';
import { MatDialog } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SaodoService } from './../../../../service/api/saodo.service';
import { AppComponentBase } from 'shared/app-component-base';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit,Input, Injector } from '@angular/core';



@Component({
  selector: 'app-sao-do-detail',
  templateUrl: './sao-do-detail.component.html',
  styleUrls: ['./sao-do-detail.component.css']
})
export class SaoDoDetailComponent extends AppComponentBase  implements OnInit {
  public listSaoDoDetail:SaodoDetailDto[]=[];
  public tempListSaoDoDetail: SaodoDetailDto []= [];
  public saodoProject={} as SaodoDetailDto;
  public searchText='';
  public saodoId:any;
  public saodoName='';
  public projectName:any;





  constructor(injector: Injector,private saodoService : SaodoService,private saodoDetailService:AuditResultService,
    private route: ActivatedRoute,private dialog: MatDialog
    , private router: Router) {super(injector)}


  ngOnInit(): void {
    this.saodoId = this.route.snapshot.queryParamMap.get('id');
    this.saodoName=this.route.snapshot.queryParamMap.get('name');
    this.getSaodoDetail()
  }
  public getSaodoDetail(): void {
    this.saodoService.getDetailById(this.saodoId , this.searchText).subscribe(data=>{
      this.listSaoDoDetail= data.result;
      this.tempListSaoDoDetail = data.result;
      this.projectName=data.result.map(el=>el.projectName)
    })
  }
  protected delete(item: SaodoDetailDto): void {
    abp.message.confirm(
      "Delete Audit " + item.projectName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.saodoDetailService.delete(item.id).pipe(catchError(this.saodoService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Audit " + item.name);
            this.getSaodoDetail();

          });
        }
      }
    );
  }
    
  showDialog(command: String, saodoProject:any): void {
     let project={};
    if (command == "edit") {
      project={
        status:this.APP_ENUM.SaodoStatus[saodoProject.auditResultStatus] ,
        pmName: saodoProject.pmName,
        projectId:saodoProject.projectId,
        id:saodoProject.id

      }
    }

    const show=this.dialog.open(CreateEditSaoDoProjectComponent, {
      data: {
        item: project,
        command: command,
        projectSaodoName:this.projectName,
      },
      width: "700px",
      disableClose: true,
      autoFocus: false
    });
    show.afterClosed().subscribe(result => {
      if(result){
        this.getSaodoDetail();
      }
    });

  }


  public createSaodoDetail(){
    this.showDialog("create",{});
  }
  public editSaodoDetail(saodo:SaodoDetailDto){
    this.showDialog("edit",saodo);

  }
  showDetail(saodoDetail:any){
    this.router.navigate(['app/saodoProjectDetail'], {
      queryParams: {
        examinationName: this.saodoName,
        projectId: saodoDetail.projectId,
        saodoId:this.saodoId,
        projectName:saodoDetail.projectName,
        id:saodoDetail.id
      }
    })
  }
  // searchName(name){
  //     this.listSaoDoDetail= this.tempListSaoDoDetail.filter((item)=>{
  //       return item.name?.toLowerCase().includes(name?.toLowerCase()) || item.pmName?.toLowerCase().includes(name?.toLowerCase())
  //     })
  // }

}
