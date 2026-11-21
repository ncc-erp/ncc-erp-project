import { Component, HostListener, Injectable, Injector, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, CanDeactivate } from '@angular/router';
import { Subject } from 'rxjs';
import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { finalize, takeUntil } from 'rxjs/operators';
import { ProjectKnowledgeBaseService } from '@app/service/api/project-knowledge-base.service';
import {
  KnowledgeBaseDto, KnowledgeBaseReferenceFileDto, KnowledgeBaseFileOptionDto
} from '@app/service/model/project-knowledge-base.dto';

@Component({
  selector: 'app-product-project-knowledge-base',
  templateUrl: './product-project-knowledge-base.component.html',
  styleUrls: ['./product-project-knowledge-base.component.css']
})
export class ProductProjectKnowledgeBaseComponent extends AppComponentBase implements OnInit, OnDestroy {
  public projectId: string;
  public knowledgeBase: KnowledgeBaseDto;
  public content = '';
  public readonly editorConfig = {
    height: 550,
    menubar: false,
    plugins: ['lists'],
    toolbar: false,
    branding: false,
    forced_root_block: 'p'
  };
  public error = '';
  public documentError = '';
  public documents: KnowledgeBaseReferenceFileDto[] = [];
  public areReferencesLoaded = false;
  public isReferencesCollapsed = true;
  public isContentCollapsed = false;
  public isSavingReferences = false;
  public isFileModalOpen = false;
  public isLoadingFiles = false;
  public areFilesLoaded = false;
  public fileOptions: KnowledgeBaseFileOptionDto[] = [];

  private originalFileIdSet = new Set<string>();
  private iconCache = new Map<string, string>();
  private destroy$ = new Subject<void>();

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private projectKnowledgeBaseService: ProjectKnowledgeBaseService
  ) {
    super(injector);
  }

  public ngOnInit(): void {
    this.projectId = this.route.snapshot.queryParamMap.get('id');
    this.getKnowledgeBase();
  }

  public get canViewKnowledgeBase(): boolean {
    return this.permission.isGranted(PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabKnowledgeBase) &&
      this.permission.isGranted(PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabKnowledgeBase_View) &&
      (this.permission.isGranted(PERMISSIONS_CONSTANT.Projects_ProductProjects_ViewAllProject) ||
        this.permission.isGranted(PERMISSIONS_CONSTANT.Projects_ProductProjects_ViewMyProjectOnly));
  }

  public get canEditKnowledgeBase(): boolean {
    return this.canViewKnowledgeBase &&
      this.permission.isGranted(PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabKnowledgeBase_Edit);
  }

  public getKnowledgeBase(): void {
    if (!this.canViewKnowledgeBase) {
      this.error = 'You do not have permission to view this knowledge base.';
      return;
    }
    this.isLoading = true;
    this.projectKnowledgeBaseService.getKnowledgeBase(this.projectId)
      .pipe(takeUntil(this.destroy$), finalize(() => this.isLoading = false))
      .subscribe(response => {
        this.knowledgeBase = response.result;
        this.content = this.knowledgeBase.content || '';
        this.documents = this.knowledgeBase.referenceFiles || [];
        this.areReferencesLoaded = true;
      }, () => this.error = 'Unable to load the knowledge base. Check the project ID or try again.');
  }

  public get isDirty(): boolean { return this.canEditKnowledgeBase && !!this.knowledgeBase && this.content !== (this.knowledgeBase.content || ''); }

  public get isSelectionDirty(): boolean {
    if (!this.isFileModalOpen || !this.areFilesLoaded) { return false; }
    let selectedCount = 0;
    for (const file of this.fileOptions) {
      if (file.isSelected) {
        selectedCount++;
        if (!this.originalFileIdSet.has(file.projectFileId)) { return true; }
      }
    }
    return selectedCount !== this.originalFileIdSet.size;
  }

  public get canGenerateKnowledgeBase(): boolean {
    return this.canEditKnowledgeBase && !!this.knowledgeBase && this.areReferencesLoaded && !this.isLoading &&
      !this.isSavingReferences && !this.isFileModalOpen;
  }

  public openFile(): void {
    if (!this.canEditKnowledgeBase || !this.knowledgeBase || this.isLoading ||
      !this.areReferencesLoaded || this.isSavingReferences || this.isFileModalOpen) { return; }

    this.isFileModalOpen = true;
    this.areFilesLoaded = false;
    this.fileOptions = [];
    this.documentError = '';
    this.isLoadingFiles = true;

    this.projectKnowledgeBaseService.getSelectableFiles(this.projectId)
      .pipe(takeUntil(this.destroy$), finalize(() => this.isLoadingFiles = false))
      .subscribe(response => {
        this.fileOptions = response.result || [];
        this.documents = this.fileOptions.filter(file => file.isSelected)
          .map(file => ({ projectFileId: file.projectFileId, name: file.name }));
        this.knowledgeBase.referenceFiles = this.documents;
        this.originalFileIdSet = new Set(this.fileOptions.filter(file => file.isSelected)
          .map(file => file.projectFileId));
        this.areFilesLoaded = true;
      }, failure => this.showDocumentError(failure, 'Could not load project files. Please try again.'));
  }

  public openFilePicker(): void {
    this.openFile();
  }

  public closeFile(): void {
    this.isFileModalOpen = false;
    this.areFilesLoaded = false;
    this.fileOptions = [];
    this.originalFileIdSet.clear();
  }

  public cancelFilePicker(): void {
    this.closeFile();
  }

  public saveReferenceFiles(): void {
    if (!this.canEditKnowledgeBase || this.isLoading || this.isSavingReferences || !this.areReferencesLoaded || !this.knowledgeBase ||
      !this.areFilesLoaded || this.isLoadingFiles || !this.isSelectionDirty) { return; }

    const ids = this.fileOptions.filter(file => file.isSelected).map(file => file.projectFileId);
    this.isSavingReferences = true;
    this.projectKnowledgeBaseService.updateReferenceFiles(this.projectId, ids)
      .pipe(takeUntil(this.destroy$), finalize(() => this.isSavingReferences = false))
      .subscribe(response => {
        this.documents = response.result || [];
        this.knowledgeBase.referenceFiles = this.documents;
        this.closeFile();
        this.notify.success('Reference files updated successfully.');
      }, failure => {
        this.showDocumentError(failure, 'Could not save the reference selection. Please try again.');
        this.notify.error(this.apiError(failure, 'Could not save the reference selection. Please try again.'));
      });
  }

  public generateKnowledgeBase(): void {
    if (!this.canGenerateKnowledgeBase) { return; }
    if (this.content && this.content.trim()) {
      abp.message.confirm(
        'AI-generated content will replace your current draft.',
        'Regenerate the knowledge base with AI?',
        confirmed => { if (confirmed) { this.runKnowledgeBaseGeneration(); } },
        { confirmButtonText: 'Yes', cancelButtonText: 'No' }
      );
      return;
    }
    this.runKnowledgeBaseGeneration();
  }

  private runKnowledgeBaseGeneration(): void {
    if (!this.canGenerateKnowledgeBase) { return; }
    this.isLoading = true;
    this.error = '';
    this.projectKnowledgeBaseService.generateKnowledgeBase(this.projectId)
      .pipe(takeUntil(this.destroy$), finalize(() => this.isLoading = false))
      .subscribe(response => this.content = response.result.content,
        failure => this.notify.error(this.apiError(failure,
          'Unable to generate the knowledge base. Please try again.')));
  }

  public saveKnowledgeBase(): void {
    if (!this.canEditKnowledgeBase || this.isLoading || !this.knowledgeBase) { return; }
    this.isLoading = true;
    this.error = '';
    this.projectKnowledgeBaseService.updateKnowledgeBase(this.projectId, this.content)
      .pipe(takeUntil(this.destroy$), finalize(() => this.isLoading = false))
      .subscribe(response => {
        this.knowledgeBase = { ...this.knowledgeBase, ...response.result };
        this.content = this.knowledgeBase.content || '';
        this.notify.success('Knowledge base saved successfully.');
      }, failure => this.notify.error(this.apiError(failure,
        'Unable to save the knowledge base. Your draft has been preserved.')));
  }

  public canLeave(): Promise<boolean> | boolean {
    if (!this.isDirty && !this.isLoading) { return true; }
    return new Promise(resolve => abp.message.confirm(
      'Unsaved content will be discarded. Do you want to leave this tab?',
      'Leave Knowledge Base?', confirmed => resolve(confirmed),
      { confirmButtonText: 'Yes', cancelButtonText: 'No' }
    ));
  }

  public trackByFileId(_index: number, item: KnowledgeBaseReferenceFileDto | KnowledgeBaseFileOptionDto): string {
    return item.projectFileId;
  }

  public iconClass(fileName: string): string {
    let cached = this.iconCache.get(fileName);
    if (cached !== undefined) { return cached; }
    const extension = (fileName || '').split('.').pop().toLowerCase();
    if (extension === 'pdf') { cached = 'fa-file-pdf-o'; }
    else if (extension === 'docx') { cached = 'fa-file-word-o'; }
    else if (extension === 'md' || extension === 'txt') { cached = 'fa-file-text-o'; }
    else { cached = 'fa-file-o'; }
    this.iconCache.set(fileName, cached);
    return cached;
  }

  private showDocumentError(failure: any, fallback: string): void {
    this.documentError = this.apiError(failure, fallback);
  }

  private apiError(failure: any, fallback: string): string {
    if (typeof failure === 'string' && failure.trim()) { return failure; }
    const message = failure && failure.error && failure.error.error && failure.error.error.message;
    if (typeof message === 'string' && message.trim()) { return message; }
    if (failure && typeof failure.message === 'string' && failure.message.trim()) { return failure.message; }
    return fallback;
  }

  @HostListener('window:beforeunload', ['$event'])
  public beforeUnload(event: BeforeUnloadEvent): void {
    if (this.isDirty || this.isLoading) {
      event.preventDefault();
      event.returnValue = '';
    }
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.closeFile();
  }
}

@Injectable({ providedIn: 'root' })
export class ProductProjectKnowledgeBaseLeaveGuard implements CanDeactivate<ProductProjectKnowledgeBaseComponent> {
  canDeactivate(component: ProductProjectKnowledgeBaseComponent) { return component.canLeave(); }
}
