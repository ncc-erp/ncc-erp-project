import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { catchError } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { ProjectdetailDto } from './../../../../../service/model/projectDetail.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { TechnologyDto } from '@app/service/model/technology.dto'; 
import { TechnologyService } from '@app/service/api/technology.service'; 
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-training-project-description',
  templateUrl: './training-project-description.component.html',
  styleUrls: ['./training-project-description.component.css']
})
export class TrainingProjectDescriptionComponent extends AppComponentBase {
  public projectId
  projectDetail = {} as ProjectdetailDto
  projectTechnologyList: any[] = []
  technologyList: TechnologyDto[] = []
  tempTechnologyList: TechnologyDto[] = []
  public searchTechnology: string = ""
  subscription: Subscription[] = [];
  Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View
  Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit
  constructor(public injector: Injector, 
    private projectService: ListProjectService, 
    private router: Router, 
    private route: ActivatedRoute,
    private technologyService: TechnologyService,) {
    super(injector)
  }
  readMode: boolean = true;
  ngOnInit(): void {
    this.projectId = this.route.snapshot.queryParamMap.get("id")
    this.getProjectDetail()
    this.getAllTechnology();
  }

  getAllTechnology() {
    this.subscription.push(
      this.technologyService.getAll().subscribe(data => {
        this.technologyList = data.result
        this.tempTechnologyList = this.technologyList
      })
    )
  }
  

  filterTechnology() {
    let selectedTechnologies = this.tempTechnologyList.filter(technology=>this.projectTechnologyList.includes(technology.id))
    this.technologyList = this.tempTechnologyList.filter(technology => technology.name.toLowerCase()
    .includes(this.searchTechnology.toLowerCase())).filter(s => !this.projectTechnologyList.includes(s.id))
         this.technologyList.unshift(...selectedTechnologies)
  }
  getProjectDetail() {
    this.projectService.getProjectDetail(this.projectId).pipe(catchError(this.projectService.handleError)).subscribe(data => {
      this.projectDetail = data.result
      this.projectTechnologyList = this.projectDetail.projectTechnologies.map(technology => technology.id)
    })
  }
  updateInfo() {
    this.isLoading = true
    this.projectDetail.projectTechnologiesInput = this.projectTechnologyList;
    this.projectService.UpdateProjectDetail(this.projectDetail).pipe(catchError(this.projectService.handleError)).subscribe(rs => {
      abp.notify.success("Update successful")
      this.isLoading = false
      this.readMode = true
      this.getProjectDetail();
    },
      () => this.isLoading = false)
  }
  editRequest() {
    this.readMode = false
  }
  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
