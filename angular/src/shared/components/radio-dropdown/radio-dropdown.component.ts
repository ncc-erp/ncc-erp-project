import { Component, Input, OnInit, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'radio-dropdown',
  templateUrl: './radio-dropdown.component.html',
  styleUrls: ['./radio-dropdown.component.css']
})
export class RadioDropdownComponent {
 @Input() data:any
 @Output() outputData = new EventEmitter<any>();
  constructor() { 
    
  }
  onSelectionChange(){
    this.outputData.emit(this.data)
  }
}
