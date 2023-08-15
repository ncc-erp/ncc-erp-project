import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import * as moment from 'moment';
import { FilterDto } from 'shared/paged-listing-component-base';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})

export class FilterComponent {
  @Input() inputFilters: InputFilterDto[];
  @Input() item: any;
  @Output() emitChange = new EventEmitter<any>();
  @Output() deleteDataFilter = new EventEmitter<any>();
  value: any;
  dropdownData:any[]=[]
  filterType:number=0;
  search: any = "";
// filterType:
// 0: text
// 1: Date
// 2: Confirm Yes/no
// 3: Dropdown
  comparisions: ComparisionDto[] = [];
  constructor() {
  }
  ngOnInit(): void {
    if (this.item.propertyName === '') {
      this.comparisions = [];
    }
    else {
      let comps = this.inputFilters.find(i => i.propertyName === this.item.propertyName)?.comparisions || [0];
      comps.forEach(element => {
        var com = new ComparisionDto();
        com.id = element;
        com.name = COMPARISIONS[element];
        this.comparisions.push(com);
      });
    }
    this.filterType = this.item.filterType
    this.dropdownData = this.item.dropdownData 

  }
  onChange(value: string | number, name: string): void {
    if (name === 'propertyName') {
      this.item.value = ''
      this.emitChange.emit({ name: 'comparision', value: undefined })
      if (value == '') {
        this.comparisions = [];
        return;
      }
      var comps = this.inputFilters.find(i => i.propertyName === value).comparisions;
      this.comparisions = [];
      comps.forEach(element => {
        var com = new ComparisionDto();
        com.id = element;
        com.name = COMPARISIONS[element];
        this.comparisions.push(com);
      });
      this.inputFilters.forEach(item => {
        if (item.propertyName == value) {
          this.filterType = item.filterType
          this.item.filterType = item.filterType
          switch(this.filterType){
            case 1:  this.item.value = moment(new Date()).format("YYYY-MM-DD")
            break;
            case 2: this.item.value =true
            break;
            case 3 : this.dropdownData = item.dropdownData,this.item.dropdownData= item.dropdownData
            break;
            case 4: this.dropdownData = item.dropdownData,this.item.dropdownData= item.dropdownData
            break;
            case 5: this.item.value =true
            break;
            case 6: this.dropdownData = item.dropdownData,this.item.dropdownData= item.dropdownData
            break;

          }
        }
        return;
      })
    }
    this.emitChange.emit({ name, value })
 
  }
  onDateChange() {
    this.item.value = moment(this.item.value).format("YYYY-MM-DD")
  }
  onRadioChange(event) {
    this.item.value = event.value
  }
  onDropdownChange(data){
    this.item.value = data
  }
  deleteFilter() {
    this.deleteDataFilter.emit();
  }
}

export class InputFilterDto {
  propertyName: string;
  displayName: string;
  comparisions: number[];
  filterType?:number;
  dropdownData?:DropDownDataDto[]
}

export class ComparisionDto {
  id: number;
  name: string;
}


export const COMPARISIONS: string[] =
  [ 'Bằng',
    'Nhỏ hơn',
    'Nhỏ hơn hoặc bằng',
    'Lớn hơn',
    'Lớn hơn hoặc bằng',
    'Không bằng',
    'Chứa kí tự',
    'Bắt đầu với',
    'Kết thúc bằng',
    'Trong']
export class DropDownDataDto{
  value: any;
  displayName:any
}