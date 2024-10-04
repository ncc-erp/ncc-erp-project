import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateCvstatusComponent } from './create-update-cvstatus/create-update-cvstatus.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-cvstatus',
  templateUrl: './cvstatus.component.html',
  styleUrls: ['./cvstatus.component.css']
})
export class CVStatusComponent implements OnInit {
  
  constructor(private dialog: MatDialog,
    injector: Injector,
  ) {
  }

  cVStatusList = [
    {
      id: 1,
      name: 'Chưa làm CV',
      color: '#232323'
    },
    {
      id: 2,
      name: 'Đã làm CV',
      color: '#543234'
    },
    {
      id: 3,
      name: 'Đang làm CV',
      color: '#234234'
    }
  ]

  ngOnInit(): void {
  }

  public showDialog() {
    const show = this.dialog.open(CreateUpdateCvstatusComponent, {
      width: "700px"
    })
  }

}
