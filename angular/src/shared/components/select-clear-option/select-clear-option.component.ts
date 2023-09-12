import { AppComponentBase } from '@shared/app-component-base';
import { Component, EventEmitter, Injector, Input, Output } from '@angular/core';

@Component({
  selector: 'app-select-clear-option',
  templateUrl: './select-clear-option.component.html',
  styleUrls: ['./select-clear-option.component.css']
})
export class SelectClearOptionComponent extends AppComponentBase  {
  @Input() listOption: number[]
  @Input() type:string = '';
  @Output()newListEvent = new EventEmitter<any>();
  @Output()doneEvent = new EventEmitter<string>();
  constructor(injector: Injector) {
    super(injector)
  }

  selectAll(){
    this.newListEvent.emit({
      type: this.type,
      data:this.listOption
    })
  }
  clear(){
    this.newListEvent.emit({
      type: this.type,
      data: []
    })
  }
  doneSelectSkill(){
    this.doneEvent.emit(this.type)
  }
}

