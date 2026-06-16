import { Component, OnInit, Inject, Injector } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { catchError } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-update-user-asset-dialog',
  templateUrl: './update-user-asset-dialog.component.html',
  styleUrls: ['./update-user-asset-dialog.component.css']
})
export class UpdateUserAssetDialogComponent extends AppComponentBase implements OnInit {
  public allAssets: any[] = [];
  public selectedAssets: number[] = [];
  public selectedAssetsCr: number[] = [];
  public isLoading: boolean = false;
  public searchAsset: string = '';
  public selectAssetList: any[] = [];
  public unSelectAssetList: any[] = [];
  subscription: Subscription[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public projectAssetService: ProjectAssetService,
    public dialogRef: MatDialogRef<UpdateUserAssetDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadAssets();
  }

  private loadAssets(): void {
    this.subscription.push(
      this.projectAssetService.GetAllForDropdown(this.data.projectId).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe(data => {
        this.allAssets = data.result || [];
        this.initializeSelectedAssets();
      })
    );
  }

  private initializeSelectedAssets(): void {
    if (this.data.currentAssets && this.data.currentAssets.length > 0) {
      this.selectedAssets = this.allAssets
        .filter(asset => this.data.currentAssets.includes(asset.id || asset.Id))
        .map(asset => asset.id || asset.Id);
      this.selectedAssetsCr = [...this.selectedAssets];
    }
    this.orderListSelect();
  }

  public openedChange(e: boolean): void {
    if (!e) {
      this.searchAsset = '';
    }
  }

  public onSelectChange(id: number): void {
    if (this.selectedAssetsCr.includes(id)) {
      this.selectedAssetsCr = this.selectedAssetsCr.filter(res => res !== id);
      this.selectedAssets = [...this.selectedAssetsCr];
    } else {
      this.selectedAssetsCr.push(id);
      this.selectedAssets = [...this.selectedAssetsCr];
    }
    this.orderListSelect();
  }

  public orderListSelect(): void {
    this.selectAssetList = this.allAssets.filter(item => this.selectedAssets.includes(item.id));
    this.unSelectAssetList = this.allAssets.filter(item => !this.selectedAssets.includes(item.id));
    this.allAssets = [...this.selectAssetList, ...this.unSelectAssetList];
  }

  public selectAll(): void {
    this.selectedAssets = this.allAssets.map(item => item.id);
    this.selectedAssetsCr = [...this.selectedAssets];
    this.orderListSelect();
  }

  public clear(): void {
    this.selectedAssets = [];
    this.selectedAssetsCr = [];
    this.orderListSelect();
  }

  public saveAndClose(): void {
    this.isLoading = true;
    this.subscription.push(
      this.projectAssetService.UpdateUserAsset(this.data.userId, this.data.projectId, this.selectedAssets).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe(() => {
        abp.notify.success('Update user assets successfully!');
        this.isLoading = false;
        this.dialogRef.close(true);
      }, () => {
        this.isLoading = false;
      })
    );
  }

  public close(): void {
    this.dialogRef.close(false);
  }

  public get filteredAssets(): any[] {
    if (!this.searchAsset) {
      return this.allAssets;
    }
    const searchTerm = this.searchAsset.toLowerCase();
    return this.allAssets.filter(asset => 
      asset.assetName.toLowerCase().includes(searchTerm)
    );
  }

  ngOnDestroy(): void {
    this.subscription.forEach(sub => sub.unsubscribe());
  }
}
