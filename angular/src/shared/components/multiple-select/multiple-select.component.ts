import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  ElementRef,
} from "@angular/core";
import { MatSelect } from "@angular/material/select";
import * as _ from "lodash";

@Component({
  selector: 'app-multiple-select',
  templateUrl: './multiple-select.component.html',
  styleUrls: ['./multiple-select.component.css']
})
export class MultipleSelectComponent implements OnInit {
  @ViewChild("matSelect") matSelect: MatSelect;
  @Input() labelName: string;
  @Input() listOption: optionDto[] = [];
  @Input() defaultValue?: number[] = [];
  @Input() disabled?: boolean;
  @Input() classCustom?: string;
  @ViewChild("search") inputSearch: ElementRef;

  @Output() onChange = new EventEmitter<any>();

  listFilteredOption: optionDto[] = [];

  listSelectedId: number[] = [];
  listSelectedIdByListOption: number[] = [];
  listSelectedIdBySearch: number[] = [];
  listSelectedIdTemp: number[] = [];

  textSearch: string = "";
  isSearch: boolean = false;

  constructor() {}

  ngOnChanges() {
    this.listFilteredOption = this.listOption;
    this.listSelectedId = this.defaultValue;
    this.getListSelectedIdByListOption();
    this.sortListFilteredOption()
  }

  ngOnInit(): void {
  }

  getListSelectedIdByListOption() {
    this.listSelectedIdByListOption = [];
    this.listSelectedIdByListOption = this.listOption.map((item) => item.id);
  }

  sortListFilteredOption() {
    let selectedOptions = this.listFilteredOption.filter(option =>
      this.listSelectedId.includes(option.id)
    );
    let unselectedOptions = this.listFilteredOption.filter(option =>
      !this.listSelectedId.includes(option.id)
    );
    this.listFilteredOption = selectedOptions.concat(unselectedOptions);
  }

  handleSearch(textSearch: string) {
    textSearch = textSearch.toLowerCase();
    if (textSearch.trim()) {
      this.isSearch = true;
      this.setListSelectedIdCache();
      this.listSelectedIdBySearch = this.listOption
        .filter((item) => item.name.toLowerCase().includes(textSearch))
        .map((item) => item.id);
    } else {
      this.isSearch = false;
      this.listSelectedId = _.cloneDeep(this.listSelectedId);
    }
    this.getListFilteredOptionBySearch(textSearch);
  }

  getListFilteredOptionBySearch(textSearch: string) {
    this.listFilteredOption = this.listOption.filter((item) =>
      item.name.toLowerCase().includes(textSearch)
    );
    this.sortListFilteredOption()
  }
  setListSelectedIdCache() {
    this.listSelectedIdTemp = this.listSelectedId;
    this.sortListFilteredOption();
  }

  handleSelectAll() {
    if (this.isSearch) {
      let intersection = this.listSelectedIdBySearch.filter(
        (x) => this.listSelectedId.indexOf(x) === -1
      );
      this.listSelectedId = this.listSelectedId.concat(intersection);
    } else {
      this.listSelectedId = this.listSelectedIdByListOption;
    }
    this.setListSelectedIdCache();
  }

  handleClear() {
    if (this.isSearch) {
      this.listSelectedId = this.listSelectedId.filter(
        (x) => this.listSelectedIdBySearch.indexOf(x) == -1
      );
    } else {
      this.listSelectedId = [];
    }
    this.setListSelectedIdCache();
  }

  onClickOption(value) {
    if (this.isSearch) {
      let indexValue = this.listSelectedIdTemp.indexOf(value);
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
    this.listSelectedIdTemp.push(value);
  }

  unSelectedOption(indexValue) {
    this.listSelectedIdTemp.splice(indexValue, 1);
  }

  setListSelectedId() {
    this.listSelectedId = this.listSelectedIdTemp;
  }
  onSelectChange() {
    this.sortListFilteredOption()
    this.onChange.emit(this.listSelectedId);
  }
  onCancelSelect() {
    this.listSelectedId = this.defaultValue;
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
export class optionDto {
  id: number;
  name: string;
}
