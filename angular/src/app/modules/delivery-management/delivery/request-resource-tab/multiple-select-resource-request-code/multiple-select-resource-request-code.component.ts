import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { MatSelect } from "@angular/material/select";
import { AppConsts } from "@shared/AppConsts";
import { APP_ENUMS } from "@shared/AppEnums";
import * as _ from "lodash";

@Component({
  selector: 'app-multiple-select-resource-request-code',
  templateUrl: './multiple-select-resource-request-code.component.html',
  styleUrls: ['./multiple-select-resource-request-code.component.css']
})
export class MultipleSelectResourceRequestCodeComponent implements OnInit {
  @ViewChild("matSelect") matSelect: MatSelect;
  @Input() labelName: string;
  @Input() listOption: resourceRequestCodeDto[] = [];
  @Input() defaultValue?: resourceRequestCodeDto[] = [];
  @Input() disabled?: boolean;
  @Input() classCustom?: string;

  @Output() onChange = new EventEmitter<any>();

  listFilteredOption: resourceRequestCodeDto[] = [];

  listSelected: resourceRequestCodeDto[] = [];
  listSelectedByListOption: resourceRequestCodeDto[] = [];
  listSelectedBySearch: resourceRequestCodeDto[] = [];
  listSelectedTemp: resourceRequestCodeDto[] = [];

  textSearch: string = "";
  isSearch: boolean = false;

  APP_ENUM = APP_ENUMS;
  APP_CONST = AppConsts;

  constructor() {}

  ngOnChanges() {
    this.listFilteredOption = this.listOption;
    this.listSelected = this.defaultValue;
    this.getListSelectedByListOption();
    this.sortListFilteredOption()
    
  }

  ngOnInit(): void {
  }

  getListSelectedByListOption() {
    this.listSelectedByListOption = [];
    this.listSelectedByListOption = [...this.listOption];
  }

  sortListFilteredOption() {
    let selectedOptions = this.listFilteredOption.filter(option =>
      this.listSelected.includes(option)
    );
    let unselectedOptions = this.listFilteredOption.filter(option =>
      !this.listSelected.includes(option)
    );
    this.listFilteredOption = selectedOptions.concat(unselectedOptions);
  }

  handleSearch(textSearch: string) {
    textSearch = textSearch.toLowerCase();
    if (textSearch.trim()) {
      this.isSearch = true;
      this.setListSelectedCache();
      this.listSelectedBySearch = this.listOption
        .filter((item) => item.code.toLowerCase().includes(textSearch));
    } else {
      this.isSearch = false;
      this.listSelected = _.cloneDeep(this.listSelected);
    }
    this.getListFilteredOptionBySearch(textSearch);
  }

  getListFilteredOptionBySearch(textSearch: string) {
    this.listFilteredOption = this.listOption.filter((item) =>
      item.code.toLowerCase().includes(textSearch)
    );
    this.sortListFilteredOption()
  }
  setListSelectedCache() {
    this.listSelectedTemp = this.listSelected;
    this.sortListFilteredOption();
  }

  handleSelectAll() {
    if (this.isSearch) {
      
      let intersection = this.listSelectedBySearch.filter(
        (x) => this.listSelected.indexOf(x) === -1
      );
      this.listSelected = this.listSelected.concat(intersection);
    } else {
      this.listSelected = this.listSelectedByListOption;
    }
    this.setListSelectedCache();
  }

  handleClear() {
    if (this.isSearch) {
      this.listSelected = this.listSelected.filter(
        (x) => this.listSelectedBySearch.indexOf(x) == -1
      );
    } else {
      this.listSelected = [];
    }
    this.setListSelectedCache();
  }

  onClickOption(value) {
    if (this.isSearch) {
      let indexValue = this.listSelectedTemp.indexOf(value);
      if (indexValue === -1) {
        this.selectedOption(value);
      } else {
        this.unSelectedOption(indexValue);
      }
      this.setListSelected();
    }
    this.sortListFilteredOption()
  }

  selectedOption(value) {
    this.listSelectedTemp.push(value);
  }

  unSelectedOption(indexValue) {
    this.listSelectedTemp.splice(indexValue, 1);
  }

  setListSelected() {
    this.listSelected = this.listSelectedTemp;
  }
  onSelectChange() {
    this.sortListFilteredOption()
    this.onChange.emit(this.listSelected);
  }
  onCancelSelect() {
    this.listSelected = this.defaultValue;
    this.matSelect.close();
    this.onSelectChange();
  }
  onOkSelect() {
    this.matSelect.close();
    this.onSelectChange();
  }
  handleOpenedChange(opened: boolean) {
    this.textSearch = "";
    this.handleSearch(this.textSearch);
  }

  compareFn(option1: any, option2: any): boolean {
    return option1 && option2 ? option1.code === option2.code && option1.status === option2.status : option1 === option2;
  }

  public getValueByEnum(enumValue: number, enumObject) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}

export class resourceRequestCodeDto {
  code: string;
  status: number;
}
