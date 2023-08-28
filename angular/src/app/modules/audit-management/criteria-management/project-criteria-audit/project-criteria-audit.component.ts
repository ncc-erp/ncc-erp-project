import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { catchError, finalize } from "rxjs/operators";
import { PagedListingComponentBase, PagedRequestDto } from "@shared/paged-listing-component-base";
import { MatDialog } from "@angular/material/dialog";
import { ProcessCriteria } from "@app/service/model/process-criteria-audit.dto";
import { SelectionModel } from "@angular/cdk/collections";
import {FlatTreeControl} from '@angular/cdk/tree';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import { ProjectProcessCriteriaAppService } from "@app/service/api/project-process-criteria.service";
import { ActivatedRoute } from "@angular/router";
import { AuditCriteriaProcessService } from "@app/service/api/audit-criteria-process.service";
import { combineLatest } from "rxjs";
import { PERMISSIONS_CONSTANT } from "@app/constant/permission.constant";


@Component({
  selector: "app-project-criteria-audit",
  templateUrl: "./project-criteria-audit.component.html",
  styleUrls: ["./project-criteria-audit.component.css"],
})
export class ProjectCriteriaAuditComponent
  extends PagedListingComponentBase<any>
  implements OnInit
{
  search:string='';
  projectId:number;
  projectName:string='';
  projectCode:string='';
  listCriteriaChecked = [] as number[]
  listCriteriaUnChecked =[]

  Audits_Tailoring_Update_Project=PERMISSIONS_CONSTANT.Audits_Tailoring_Update_Project
  Audits_Tailoring_Update_Project_Tailoring=PERMISSIONS_CONSTANT.Audits_Tailoring_Update_Project_Tailoring
  Audits_Tailoring_Detail= PERMISSIONS_CONSTANT.Audits_Tailoring_Detail

  flatNodeMap = new Map<TodoItemFlatNode, PeriodicElement>();
  nestedNodeMap = new Map<PeriodicElement, TodoItemFlatNode>();

  selectedParent: TodoItemFlatNode | null = null;
  checklistSelection = new SelectionModel<TodoItemFlatNode>(true /* multiple */);
  treeControl: FlatTreeControl<TodoItemFlatNode>;
  treeFlattener: MatTreeFlattener<PeriodicElement, TodoItemFlatNode>;
  dataSource: MatTreeFlatDataSource<PeriodicElement, TodoItemFlatNode>;
  getLevel = (node: TodoItemFlatNode) => node.level;
  isExpandable = (node: TodoItemFlatNode) => node.expandable;
  getChildren = (node: PeriodicElement): PeriodicElement[] => node.children;
  hasChild = (_: number, _nodeData: TodoItemFlatNode) => _nodeData.expandable;


  /**
   * Transformer to convert nested node to flat node. Record the nodes in maps for later use.
   */
  transformer = (node: PeriodicElement, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.item === node.item
        ? existingNode
        : new TodoItemFlatNode();
    flatNode.item = node.item;
    flatNode.level = level;
    flatNode.expandable = !!node.children && node.children.length > 0;
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  descendantsAllSelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    return descAllSelected;
  }

  /** Whether part of the descendants are selected */
  descendantsPartiallySelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  /** Toggle the to-do item selection. Select/deselect all the descendants node */
  todoItemSelectionToggle(node: TodoItemFlatNode,callApi:boolean): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);

    if(this.checklistSelection.isSelected(node)){
      this.listCriteriaChecked = [...descendants,node].map(res=>{return res.item.id})
      this.checklistSelection.select(...descendants)

    }
    else{
      this.checklistSelection.deselect(...descendants);
      this.listCriteriaUnChecked = [...descendants,node].map(res=>{return res.item.id})
    }


    // Force update for the parent
    descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    this.checkAllParentsSelection(node,callApi);
  }

  /** Toggle a leaf to-do item selection. Check all the parents to see if they changed */
  todoLeafItemSelectionToggle(node: TodoItemFlatNode,callApi:boolean): void {
    this.checklistSelection.toggle(node);
    if(this.checklistSelection.isSelected(node)){
      this.listCriteriaChecked= [...this.listCriteriaChecked,node.item.id]
      this.checklistSelection.select(node)
    }
    else{
      this.listCriteriaUnChecked= [...this.listCriteriaUnChecked,node.item.id]
      this.checklistSelection.deselect(node);

    }
    this.checkAllParentsSelection(node,callApi);
  }

  /* Checks all the parents when a leaf node is selected/unselected */
  checkAllParentsSelection(node: TodoItemFlatNode,callApi:boolean): void {
    let parent: TodoItemFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  if(!parent){
  if(!callApi){
    this.listCriteriaUnChecked=[]
    this.listCriteriaChecked=[]
  }
  else{
  if(this.listCriteriaChecked.length>0 && callApi){
    this.processProjectService.addMultiCriteriaToOneProject(this.projectId,this.listCriteriaChecked).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
      abp.notify.success("Update Successfully!");
      this.listCriteriaChecked=[]
      this.listCriteriaUnChecked=[]
  })
  }
  if(this.listCriteriaUnChecked.length>0 && callApi){
    this.processProjectService.deleteCriteria(this.projectId,this.listCriteriaUnChecked).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
      abp.notify.success("Update Successfully!");
      this.listCriteriaUnChecked=[]
      this.listCriteriaChecked=[]
  })}
 }

}

  }


  /** Check root node checked state and change it accordingly */
  checkRootNodeSelection(node: TodoItemFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    if (nodeSelected && !descAllSelected) {
      this.listCriteriaUnChecked=[...this.listCriteriaUnChecked,node.item.id]
      this.checklistSelection.deselect(node);

    } else if (!nodeSelected && descAllSelected) {
      this.listCriteriaChecked=[...this.listCriteriaChecked,node.item.id]
      this.checklistSelection.select(node);
    }
  }

  getParentNode(node: TodoItemFlatNode): TodoItemFlatNode | null {
    const currentLevel = this.getLevel(node);
    if (currentLevel < 1) {
      return null;
    }
    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];
      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

  constructor(injector: Injector, private dialog: MatDialog,
    private processProjectService: ProjectProcessCriteriaAppService,
    private processCriteriaService: AuditCriteriaProcessService ,
    route:ActivatedRoute) {
    super(injector);
    this.projectId = Number(route.snapshot.queryParamMap.get("projectId"));
    this.projectCode = route.snapshot.queryParamMap.get("projectCode");
    this.projectName = route.snapshot.queryParamMap.get("projectName");
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
      this.isExpandable, this.getChildren);
      this.treeControl = new FlatTreeControl<TodoItemFlatNode>(this.getLevel, this.isExpandable);
      this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener)
      const allProcessCriteria = this.processCriteriaService.getCriteriaActive()
      const projectProcess = this.processProjectService.getProcessCriteriaByProjectId(this.projectId)
      combineLatest(allProcessCriteria,projectProcess).pipe(catchError(this.processCriteriaService.handleError))
      .subscribe(([allProcessCriteria,projectProcess])=>{
        this.treeControl.dataNodes=allProcessCriteria.result.childrens;
        this.dataSource.data=allProcessCriteria.result.childrens;
        this.treeControl.expandAll()
        for(let itemG of this.treeControl.dataNodes) {
          if(projectProcess.result.some(item=> {
            return item.id==itemG.item.id
          })){
            this.checklistSelection.toggle(itemG);
            this.checkAllParentsSelection(itemG,false);
          }
        }
    })

};


  ngOnInit(): void {

  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {}

  protected delete(entity: any): void {
    throw new Error("Method not implemented.");
  }

  view(){

    if(this.permission.isGranted(this.Audits_Tailoring_Detail)){
      this.router.navigate(['/app/tailoring-project'], {
        queryParams: {
          projectId:this.projectId,
          projectCode: this.projectCode,
          projectName:this.projectName
        }
      })
    }

}
  searchCriteria() {
  const searchProcessCriteria = this.processCriteriaService.searchCriteriaActive(this.search)
  const projectProcess = this.processProjectService.getProcessCriteriaByProjectId(this.projectId)
      combineLatest(searchProcessCriteria,projectProcess).pipe(catchError(this.processCriteriaService.handleError))
      .subscribe(([searchProcessCriteria,projectProcess])=>{
        this.treeControl.dataNodes=searchProcessCriteria.result.childrens;
        this.dataSource.data=searchProcessCriteria.result.childrens;
        this.treeControl.expandAll()
        for(let itemG of this.treeControl.dataNodes) {
          if(projectProcess.result.some(item=> {
            return item.id==itemG.item.id
          })){
            this.checklistSelection.toggle(itemG);
            this.checkAllParentsSelection(itemG,false);
          }
        }
     })
}
}


export class PeriodicElement {
  item: ProcessCriteria
  children?: PeriodicElement[];
}
export class TodoItemFlatNode {
  item: ProcessCriteria;
  expandable: boolean;
  level:number
}


