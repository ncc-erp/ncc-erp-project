import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ProjectFileService } from '@app/service/api/project-file.service';
import { ProjectFileDto } from '@app/service/model/projectFile.dto';
import { AppComponentBase } from '@shared/app-component-base';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-product-project-file',
  templateUrl: './product-project-file.component.html',
  styleUrls: ['./product-project-file.component.css']
})
export class ProductProjectFileComponent extends AppComponentBase implements OnInit {
  private projectId: number
  public fileList: any[] = []
  public isLoading: boolean = false

  Projects_ProductProjects_ProjectDetail_TabProjectFile_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectFile_View
  Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile
  Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile

  constructor(private projectFileService: ProjectFileService, private route: ActivatedRoute, injector: Injector) {
    super(injector)
    this.projectId = Number(route.snapshot.queryParamMap.get("id"))
  }

  ngOnInit(): void {
    this.getAllFile()
  }
  private getAllFile() {

    this.projectFileService.getAllFile(this.projectId).subscribe(data => {
      this.fileList = data.result
    })
  }


  public selectFile(event) {
    let fileData = this.fileList.map(item => {
      let file = new Blob([this.converFile(atob(item.bytes))], {
        type: ""
      });
      let arrayOfBlob = new Array<Blob>();
      arrayOfBlob.push(file);
      item = new File(arrayOfBlob, item.fileName)
      return item;
    })
    fileData.push(...event.target.files);
    if (!this.fileList) {
      abp.message.error("Choose a file!")
      return
    }
    this.isLoading = true

    this.projectFileService.UploadFiles(fileData, this.projectId)
      .subscribe((res) => {
        abp.notify.success("Upload file successful")
        this.getAllFile()
        this.isLoading = false
      },
        () => { this.isLoading = false });
  }
  private converFile(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
  private downloadFile(projectFile: ProjectFileDto) {
    const file = new Blob([this.converFile(atob(projectFile.bytes))], {
      type: ""
    });
    FileSaver.saveAs(file, projectFile.fileName);
  }
  removeFile(file: ProjectFileDto) {
    abp.message.confirm(
      "Remove file " + file.fileName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.projectFileService.removeFile(file.fileName, this.projectId).subscribe(rs => {
            abp.notify.success(`remove file ${file.fileName}`)
            this.getAllFile()
          })
        }
      }
    )
  }
}
