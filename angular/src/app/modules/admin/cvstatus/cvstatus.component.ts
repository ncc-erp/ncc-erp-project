import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cvstatus',
  templateUrl: './cvstatus.component.html',
  styleUrls: ['./cvstatus.component.css']
})
export class CVStatusComponent implements OnInit {
  
  constructor() {
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

}
