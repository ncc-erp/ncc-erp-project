import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  ViewChild,
  ElementRef
} from "@angular/core";
import { MatSelect } from "@angular/material/select";
import * as _ from "lodash";

@Component({
  selector: 'app-multiple-select-string-value',
  templateUrl: './multiple-select-string-value.component.html',
  styleUrls: ['./multiple-select-string-value.component.css']
})
export class MultipleSelectStringValueComponent implements OnInit, OnChanges {
  @ViewChild("matSelect") matSelect: MatSelect;
  @Input() labelName: string;
  @Input() listOption: string[] = [];
  @Input() defaultValue?: string[] = [];
  @Input() classCustom?: string;
  @Input() disabled?: boolean;

  @Output() onChange = new EventEmitter<any>();
  @ViewChild('search') inputSearch: ElementRef;

  listFilteredOption: string[] = [];

  listSelected: string[] = [];
  listSelectedByListOption: string[] = [];
  listSelectedBySearch: string[] = [];
  listSelectedTemp: string[] = [];

  textSearch: string = "";
  isSearch: boolean = false;

  constructor() {}

  ngOnChanges() {
    this.listFilteredOption = this.listOption;
    this.listSelected = this.defaultValue;
    this.getListSelectedIdByListOption();
    this.sortListFilteredOption()
  }

  ngOnInit(): void {
  }

  getListSelectedIdByListOption() {
    this.listSelectedByListOption = [];
    this.listSelectedByListOption = this.listOption.slice();
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
      this.setListSelectedIdCache();
      this.listSelectedBySearch = this.listOption
        .filter((item) => item.toLowerCase().includes(textSearch))
        .slice();
    } else {
      this.isSearch = false;
      this.listSelected = _.cloneDeep(this.listSelected);
    }
    this.getListFilteredOptionBySearch(textSearch);
  }

  getListFilteredOptionBySearch(textSearch: string) {
    this.listFilteredOption = this.listOption.filter((item) =>
      item.toLowerCase().includes(textSearch)
    );
    this.sortListFilteredOption()
  }
  setListSelectedIdCache() {
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
    this.setListSelectedIdCache();
  }

  handleClear() {
    if (this.isSearch) {
      this.listSelected = this.listSelected.filter(
        (x) => this.listSelectedBySearch.indexOf(x) == -1
      );
    } else {
      this.listSelected = [];
    }
    this.setListSelectedIdCache();
  }

  onClickOption(value) {
    if (this.isSearch) {
      let indexValue = this.listSelectedTemp.indexOf(value);
      if (indexValue === -1) {
        this.selectedOption(value);
      } else {
        this.unSelectedOption(indexValue);
      }
      this.setListSelectedId();
    }
    this.sortListFilteredOption()
  }

  selectedOption(value) {
    this.listSelectedTemp.push(value);
  }

  unSelectedOption(indexValue) {
    this.listSelectedTemp.splice(indexValue, 1);
  }

  setListSelectedId() {
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
    this.inputSearch.nativeElement.focus();
    this.handleSearch(this.textSearch);
  }
}

