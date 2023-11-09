import { Component, OnInit, Input, Output, EventEmitter, Injector } from '@angular/core';
import * as moment from 'moment';
import { FilterDto } from 'shared/paged-listing-component-base';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})

export class FilterComponent extends AppComponentBase{
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

//filter Comparison
  equal: string;
  smaller: string;
  smallerOrEqual: string;
  bigger: string;
  biggerOrEqual: string;
  notEqual: string;
  contain: string;
  startWith: string;
  endWith: string;
  in: string;
  comparisions: ComparisionDto[] = [];

  constructor( private injector: Injector,) {
    super(injector)
  }

  ngOnInit(): void {
    this.injectComparison();
    if (this.item.propertyName === '') {
      this.comparisions = [];
    }
    else {
      const comps = this.inputFilters.find(i => i.propertyName === this.item.propertyName)?.comparisions || [0];
      this.comparisions = comps.map(element => {
        return {
          id: element,
          name: COMPARISIONS[element],
        };
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

  injectComparison() : void {
    this.equal = this.l("Equal");
    this.smaller = this.l("Smaller");
    this.smallerOrEqual = this.l("Smaller_or_Equal");
    this.bigger = this.l("Bigger");
    this.biggerOrEqual = this.l("Bigger_or_Equal");
    this.notEqual = this.l("Not_Equal");
    this.contain = this.l("Contains");
    this.startWith = this.l("Start_With");
    this.endWith = this.l("End_With");
    this.in = this.l("In");

    COMPARISIONS[0] = this.equal;
    COMPARISIONS[1] = this.smaller;
    COMPARISIONS[2] = this.smallerOrEqual;
    COMPARISIONS[3] = this.bigger;
    COMPARISIONS[4] = this.biggerOrEqual;
    COMPARISIONS[5] = this.notEqual;
    COMPARISIONS[6] = this.contain;
    COMPARISIONS[7] = this.startWith;
    COMPARISIONS[8] = this.endWith;
    COMPARISIONS[9] = this.in;
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
  [
    'Equal',
    'Smaller',
    'Smaller_or_Equal',
    'Bigger',
    'Bigger_or_Equal',
    'Not_Equal',
    'Contains',
    'Start_With',
    'End_With',
    'In',
  ]
export class DropDownDataDto{
  value: any;
  displayName:any
}
