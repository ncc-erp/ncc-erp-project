import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges, AfterViewInit, ViewChild, QueryList, ViewChildren, ElementRef } from '@angular/core';
import { DropDownDataDto } from '@shared/filter/filter.component';

@Component({
  selector: 'app-multi-select-option',
  templateUrl: './multi-select-option.component.html',
  styleUrls: ['./multi-select-option.component.css']
})
export class MultiSelectOptionComponent implements OnInit, OnChanges {
  @Input() searchPlaceHolder = ''
  @Input() dropdownData: DropDownDataDto[] = []
  @Input() selectedValues: any[] = []
  @Input() selectedValue: any 
  @Input() multiple: boolean = true
  @Input() selectLabel: string = 'Select'
  @Input() required:  boolean = false;
  @Output() onMultiSelectionChange: EventEmitter<any[]> = new EventEmitter<any[]>()
  @Output() onSingleSelectionChange: EventEmitter<any[]> = new EventEmitter<any[]>()
  @ViewChildren('search') inputSearch: QueryList<ElementRef>;
  public searchString: string = ''
  public tempData: DropDownDataDto[]
  constructor() { }
  ngOnChanges(changes: SimpleChanges): void {
    if('dropdownData' in changes){
      this.dropdownData = changes.dropdownData.currentValue
      this.tempData = [...this.dropdownData]
      this.searchString = ''
    }
  }

  ngOnInit(): void {
    this.tempData = [...this.dropdownData]
  }

  onSearch(value: string){
    if(this.searchString){
      this.tempData = this.dropdownData.filter(item => item.displayName.toLowerCase().includes(this.searchString.toLowerCase()))
      return;
    }
    this.tempData = [...this.dropdownData]
  }

  onSingleSelectChange(event: any){
    this.onSingleSelectionChange.emit(event);
  }

  onMultiSelectChange(event: any[]){
    this.onMultiSelectionChange.emit(event)
  }

  onOpenedChange(isOpened: boolean){
    if(!isOpened && this.searchString && !this.tempData.length) {
      this.tempData = [...this.dropdownData]
      this.searchString = ''
    }
    this.inputSearch.forEach(ref => {
      ref.nativeElement.focus()
    })
  }
}