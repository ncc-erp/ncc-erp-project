import { AppComponentBase } from "@shared/app-component-base";
import { Component, Inject, Injector, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { catchError } from "rxjs/operators";
import { DomSanitizer } from "@angular/platform-browser";
import { ProjectProcessCriteriaResultAppService } from "@app/service/api/project-process-criteria-result.service";
import { GetProjectProcessCriteriaResultDto } from '@app/service/model/project-process-criteria-result.dto'

import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-edit-audit-result-detail',
  templateUrl: './edit-audit-result-detail.component.html',
  styleUrls: ['./edit-audit-result-detail.component.css']
})
export class EditAuditResultDetailComponent extends AppComponentBase implements OnInit {

  projectProcessCriteriaResult = {} as GetProjectProcessCriteriaResultDto;
  isUpdateCriteria = true;

  stateCtrl = new FormControl();
  filteredStates: Observable<State[]>;

  states: State[] = [
    {
      name: 'Arkansas',
      population: '2.978M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Arkansas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/9/9d/Flag_of_Arkansas.svg'
    },
    {
      name: 'California',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg'
    },
    {
      name: 'Florida',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg'
    },
    {
      name: 'Texas',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Texas.svg'
    }
  ];


  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<EditAuditResultDetailComponent>,
    private projectProcessCriteriaResultAppService: ProjectProcessCriteriaResultAppService,
    private sanitizer: DomSanitizer,
    injector: Injector,
    fb: FormBuilder
  ) {
    super(injector);
    this.filteredStates = this.stateCtrl.valueChanges
      .pipe(
        startWith(''),
        map(state => state ? this._filterStates(state) : this.states.slice())
      );
  }

  ngOnInit() {

  }

  private _filterStates(value: string): State[] {
    const filterValue = value.toLowerCase();

    return this.states.filter(state => state.name.toLowerCase().indexOf(filterValue) === 0);
  }

  configEditor: any = {
    height: 250,
    theme: "modern",
    plugins:
      'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image imagetools link media template codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists textcolor wordcount contextmenu colorpicker textpattern',
    toolbar:
      'formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat | image',
    image_advtab: true,
    imagetools_toolbar:
      'rotateleft rotateright | flipv fliph | editimage imageoptions',
    templates: [
      { title: 'Test template 1', content: 'Test 1' },
      { title: 'Test template 2', content: 'Test 2' },
    ],
    content_css: [
      '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
      '//www.tinymce.com/css/codepen.min.css',
    ],
    setup: (editor) => {
      console.log(editor.ui);
    },
  };

}

export interface State {
  flag: string;
  name: string;
  population: string;
}
